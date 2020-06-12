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
    
    if (!(array_key_exists("auth", $post) && array_key_exists("username", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
    $other_user = $connection->realEscapeString($post["username"]);
	
	$result = $connection->select("user", ["username"], "`username` = '{$other_user}'");
	if (sizeof($result) == 0)
		throw new ObjectDoesntExistException();
	
	$result = $connection->select("game", [], "`player_1` = '{$username}' AND `player_2` = '{$other_user}' AND `won_by` IS NULL");
	if (sizeof($result) > 0) // sender has already sent a request before
		return ["result" => "ok"];
    
	$result = $connection->select("game", ["id"], "`player_1` = '{$other_user}' AND `player_2` = '{$username}' AND `won_by` IS NULL");
	if (sizeof($result) == 0) { // reciever has not sent a request to sender
		$data = [];
		$data[] = ["player_1" => $username, "player_2" => $other_user];
		$connection->insert("game", $data);
		return ["result" => "ok"];
	}
	$game_id = $result[0]["id"];
	$result = $connection->select("round", [], "`game` = '{$game_id}'");
	if (sizeof($result) == 0)// sender is answering a request, and the game needs to be initialized
		Globals::initializeNewRound($connection, $game_id);
	
	$result = ["result" => "ok"];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    