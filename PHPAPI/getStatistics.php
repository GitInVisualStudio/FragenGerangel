<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/WrongCredentialsException.php");
require_once("Exceptions/WrongFormatException.php");
require_once("Exceptions/ServerException.php");
require_once("Exceptions/InsufficientPermissionsException.php");

set_error_handler(function($errno, $errstr, $errfile, $errline, $errcontext) {
    // error was suppressed with the @-operator
    if (0 === error_reporting()) {
        return false;
    }

    throw new Exception($errstr, $errno);
});

function main(array $post) : array {
    $connection = Globals::getDBConnection();
    
    if (!(array_key_exists("auth", $post) && array_key_exists("username", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$username = $connection->realEscapeString($post["username"]);
	
	$result = $connection->select("user", ["elo"], "username = '{$username}'");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$elo = $result["elo"];
	
	$result = $connection->query("SELECT COUNT(*) AS num FROM game g WHERE (SELECT COUNT(*) FROM game gg INNER JOIN `round` r ON r.game = gg.id INNER JOIN question_answer a ON a.`round` = r.id WHERE g.id = gg.id AND ((gg.player_1 = '{$username}' AND a.answer_player_1 = 1) OR (gg.player_2 = '{$username}' AND answer_player_2 = 1))) = 18");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$perfect_games = $result["num"];
	
	$result = $connection->query("SELECT COUNT(*) AS num FROM game WHERE (player_1 = '{$username}' AND won_by = 1) OR (player_2 = '{$username}' AND won_by = 2)");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$wins = $result["num"];
	
	$result = $connection->query("SELECT COUNT(*) AS num FROM game WHERE (player_1 = '{$username}' AND won_by = 2) OR (player_2 = '{$username}' AND won_by = 1);");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$losses = $result["num"];
	
	$result = $connection->query("SELECT COUNT(*) AS num FROM game WHERE (player_1 = '{$username}' OR player_2 = '{$username}') AND won_by = 0");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$draws = $result["num"];
	
	$result = $connection->query("SELECT DISTINCT category FROM question");
	$percentages = [];
	for ($i = 0; $i < sizeof($result); $i++) {
		$category = $result[$i]["category"];
		$answers = $connection->query("SELECT
								g.player_1, g.player_2, a.answer_player_1, a.answer_player_2
								FROM game g INNER JOIN `round` r ON r.game = g.id INNER JOIN question_answer a ON a.`round` = r.id INNER JOIN question q ON a.question = q.id
								WHERE q.category = '{$category}' AND (player_1 = '{$username}' OR player_2 = '{$username}')");
		$all_answers = sizeof($result);
		$correct_answers = 0;
		foreach ($answers as $answer) 
			if (($answer["player_1"] == $username && $answer["answer_player_1"] == 0) || ($answer["player_2"] == $username && $answer["answer_player_2"] == 0))
				$correct_answers++;
		$percentages[$i]["category"] = $category;
		$percentages[$i]["percentage"] = $all_answers = 0 ? 1 : $correct_answers / $all_answers;
	}
	// TODO: category percentages
	
	$result = ["result" => "ok", "elo" => $elo, "perfectGames" => $perfect_games, "wins" => $wins, "draws" => $draws, "losses" => $losses, "categoryPercentages" => $percentages];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    