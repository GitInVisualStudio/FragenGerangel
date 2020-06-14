<?php
require_once("Utils/DBConnection.php");
require_once("Utils/Globals.php");
require_once("Exceptions/MissingParameterException.php");
require_once("Exceptions/WrongCredentialsException.php");
require_once("Exceptions/WrongFormatException.php");

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
    
	//var_dump($connection->query("SELECT * FROM game g WHERE g.player_1 = '{$other_user}' AND g.player_2 = '{$username}' AND NOT EXISTS(SELECT * FROM `round` r WHERE r.game = g.id)");
	$connection->query("DELETE FROM game WHERE player_1 = '{$other_user}' AND player_2 = '{$username}' AND id NOT IN (SELECT DISTINCT game FROM `round`)");
	
    return ["result" => "ok"];
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    