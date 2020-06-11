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
    
    if (!(array_key_exists("auth", $post) && array_key_exists("gameID", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$game_id = $connection->realEscapeString($post["gameID"]);
	
	$result = $connection->select("game", [], "`id` = '{$game_id}' AND (`player_1` = '{$username}' OR `player_2` = '{$username}')");
	if (sizeof($result) == 0)
		throw new InsufficientPermissionsException();
	
	$result = $connection->select("round", [], "`game` = '{$game_id}'", "`order` ASC");
	$rounds = [];
	for ($i = 0; $i < sizeof($result); $i++) {
		$row = $result[$i];
		$rounds[$i] = [];
		$round =& $rounds[$i];
		$round["id"] = $row["id"];
		$round["order"] = $row["order"];
		$round["cat1"] = $row["cat_1"];
		$round["cat2"] = $row["cat_2"];
		$round["cat3"] = $row["cat_3"];
		$round["category"] = $row["category"];
	}
	
	$result = ["result" => "ok", "rounds" => $rounds];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    