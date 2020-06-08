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
    
    if (!(array_key_exists("auth", $post) && array_key_exists("id", $post) && array_key_exists("answer", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$qa_id = $connection->realEscapeString($post["id"]);
	$answer = $connection->realEscapeString($post["answer"]);
	if ($answer < 0 || $answer > 4)
		throw new WrongFormatException();
	
	$result = $connection->query("SELECT g.player_1 AS player_1, g.player_2 AS player_2 FROM question_answer q INNER JOIN `round` r ON q.`round` = r.id INNER JOIN game g ON r.game = g.id WHERE g.player_1 = '{$username}' OR g.player_2 = '{$username}'");
	if (sizeof($result) == 0)
		throw new InsufficientPermissionsException();
	
	$result = $result[0];
	$player_1 = $result["player_1"];
	$player_2 = $result["player_2"];
	$player = $player_1 == $username ? "player_1" : "player_2";
	$connection->query("UPDATE question_answer SET answer_{$player} = '{$answer}' WHERE `id` = '{$qa_id}' AND answer_{$player} IS NULL");
	
	$result = $connection->query("SELECT g.id AS game_id, r.id AS round_id FROM question_answer q INNER JOIN `round` r ON q.`round` = r.id INNER JOIN game g ON r.game = g.id WHERE q.id = '{$qa_id}'");
	if (sizeof($result) != 1)
		throw new ServerException();
	$result = $result[0];
	$round_id = $result["round_id"];
	$game_id = $result["game_id"];
	
	$result = $connection->query("SELECT * FROM `round` r INNER JOIN question_answer q ON q.`round` = r.id WHERE r.id = '{$round_id}' AND (answer_player_1 IS NULL OR answer_player_2 IS NULL)");
	if (sizeof($result) > 0)
		return ["result" => "ok"];
	
	$result = $connection->query("SELECT * FROM game g INNER JOIN `round` r ON r.game = g.id WHERE g.id = '{$game_id}'");
	if (sizeof($result) == 6) {
		$result = $connection->query("SELECT * FROM question_answer q INNER JOIN `round` r ON q.`round` = r.id INNER JOIN game g ON r.game = g.id WHERE q.answer_player_1 = 0 AND g.id = '{$game_id}';");
		$score_player_1 = sizeof($result);
		$result = $connection->query("SELECT * FROM question_answer q INNER JOIN `round` r ON q.`round` = r.id INNER JOIN game g ON r.game = g.id WHERE q.answer_player_2 = 0 AND g.id = '{$game_id}';");
		$score_player_2 = sizeof($result);
		if ($score_player_1 == $score_player_2)
			$won_by = 0;
		else if ($score_player_1 > $score_player_2)
			$won_by = 1;
		else
			$won_by = 2;
		
		$result = $connection->select("user", ["username", "elo"], "username in ('{$player_1}', '{$player_2}')");
		if (sizeof($result) != 2)
			throw new ServerException();
		
		$elo_player_1 = $result[0]["username"] == $player_1 ? $result[0]["elo"] : $result[1]["elo"];
		$elo_player_2 = $result[1]["username"] == $player_2 ? $result[1]["elo"] : $result[0]["elo"];
		$delta_elo = $elo_player_1 - $elo_player_2;
		$expected_score_player_1 = 1 / (1 + pow(10, $delta_elo / 400));
		$expected_score_player_2 = 1 - $expected_score_player_1;
		$actual_score_player_1 = $won_by == 1 ? 1 : $won_by == 0 ? 0.5 : 0;
		$actual_score_player_2 = 1 - $actual_score_player_1;
		$k = 30;
		$delta_elo_player_1 = $k * ($actual_score_player_1 - $expected_score_player_1);
		$delta_elo_player_2 = $k * ($actual_score_player_2 - $expected_score_player_2);
		$connection->query("UPDATE user SET elo = elo + {$delta_elo_player_1} WHERE username = '{$player_1}'");
		$connection->query("UPDATE user SET elo = elo + {$delta_elo_player_2} WHERE username = '{$player_2}'");
		
		$connection->query("UPDATE game SET won_by = {$won_by} WHERE id = '{$game_id}'");
		//print("updated elos. \nexpected score p1: {$expected_score_player_1}. actual score p1: {$actual_score_player_1}. delta elo: {$delta_elo_player_1}\nexpected score p2: {$expected_score_player_2}. actual score p2: {$actual_score_player_2}. delta elo: {$delta_elo_player_2}\n");
		return ["result" => "ok", $delta_elo => $username == $player_1 ? $delta_elo_player_1 : $delta_elo_player_2];
	}
	
	Globals::initializeNewRound($connection, $game_id);
	
	$result = ["result" => "ok"];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    