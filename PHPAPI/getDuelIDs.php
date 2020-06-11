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
	
	$result = $connection->select("game` `g", ["id", "player_1", "player_2"], "(`player_1` = '{$username}' OR `player_2` = '{$username}') AND EXISTS(SELECT * FROM round r WHERE g.id = r.game)");
	$games = [];
	for ($i = 0; $i < sizeof($result); $i++) {
		$row = $result[$i];
		$games[$i] = [];
		$games[$i]["gameID"] = $row["id"];
		$games[$i]["username"] = $row["player_1"] == $username ? $row["player_2"] : $row["player_1"];
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
    