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

function start(array $post) : array {
    $connection = Globals::getDBConnection();
    
    if (!(array_key_exists("auth", $post) && array_key_exists("roundID", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$round_id = $connection->realEscapeString($post["roundID"]);
	
	$result = $connection->query("SELECT q.id AS id, q.`order` AS `order`, q.question AS question, q.answer_player_1 AS answer_player_1, q.answer_player_2 AS answer_player_2, g.player_1 AS player_1, g.player_2 AS player_2 FROM `question_answer` q INNER JOIN `round` r ON q.round = r.id INNER JOIN game g ON r.game = g.id WHERE r.id = '{$round_id}' AND (`player_1` = '{$username}' OR `player_2` = '{$username}') ORDER BY `order` ASC");
	if (sizeof($result) == 0)
		throw new InsufficientPermissionsException();
	
	$qas = [];
	for ($i = 0; $i < sizeof($result); $i++) {
		$row = $result[$i];
		$qas[$i] = [];
		$qa =& $qas[$i];
		$qa["id"] = $row["id"];
		$qa["order"] = $row["order"];
		$qa["questionID"] = $row["question"];
		$qa["yourAnswer"] = $row["player_1"] == $username ? $row["answer_player_1"] : $row["answer_player_2"];
		$qa["enemyAnswer"] = $row["player_1"] == $username ? $row["answer_player_2"] : $row["answer_player_1"];
	}
	$result = ["result" => "ok", "questionAnswers" => $qas];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    