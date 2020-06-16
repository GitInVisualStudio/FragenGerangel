<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/WrongCredentialsException.php");
require_once("Exceptions/WrongFormatException.php");
require_once("Exceptions/ServerException.php");

set_error_handler(function($errno, $errstr, $errfile, $errline, $errcontext) {
    // error was suppressed with the @-operator
    if (0 === error_reporting()) {
        return false;
    }

    throw new Exception($errstr, $errno);
});

function start(array $post) : array {
    $connection = Globals::getDBConnection();
    
    if (!(array_key_exists("auth", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	
	$result = $connection->select("game` `g", ["id", "player_1", "player_2", "won_by"], "(`player_1` = '{$username}' OR `player_2` = '{$username}') AND EXISTS(SELECT * FROM round r WHERE g.id = r.game)");
	$games = [];
	for ($i = 0; $i < sizeof($result); $i++) {
		$row = $result[$i];
		$player_1 = $row["player_1"] == $username;
		$games[$i] = [];
		$games[$i]["gameID"] = $row["id"];
		$games[$i]["username"] = $player_1 ? $row["player_2"] : $row["player_1"];
		$games[$i]["active"] = $row["won_by"] == null ? true : false;
		
		if ($games[$i]["active"]) {
			$result2 = $connection->query("SELECT g.player_1 AS player_1, g.player_2 AS player_2, q.answer_player_1 AS answer_player_1, q.answer_player_2 AS answer_player_2 FROM game g INNER JOIN `round` r ON r.game = g.id LEFT JOIN question_answer q ON q.`round` = r.id WHERE g.id = '{$games[$i]["gameID"]}' ORDER BY r.`order`, q.`order`");
			$row2 = $result2[0];
			$games[$i]["yourTurn"] = ($player_1 && $row2["answer_player_1"] == null) || (!$player_1 && $row2["answer_player_2"] == null);
		}
		$result2 = $connection->query("SELECT * FROM game g INNER JOIN `round` r ON r.game = g.id INNER JOIN question_answer q ON q.`round` = r.id WHERE g.id = '{$games[$i]["gameID"]}' AND answer_player_1 = 0 AND answer_player_2 IS NOT null");
		$score_player_1 = sizeof($result2);
		$result2 = $connection->query("SELECT * FROM game g INNER JOIN `round` r ON r.game = g.id INNER JOIN question_answer q ON q.`round` = r.id WHERE g.id = '{$games[$i]["gameID"]}' AND answer_player_2 = 0 AND answer_player_1 IS NOT null");
		$score_player_2 = sizeof($result2);
		$games[$i]["yourScore"] = $player_1 ? $score_player_1 : $score_player_2;
		$games[$i]["enemyScore"] = $player_1 ? $score_player_2 : $score_player_1;
	}
	
	$result = ["result" => "ok", "games" => $games];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    