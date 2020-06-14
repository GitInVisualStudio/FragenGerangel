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
    
    if (!(array_key_exists("auth", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	
	$result = $connection->query("SELECT * FROM user_is_friends WHERE accepted = 1 AND (sender = '{$username}' OR reciever = '{$username}')");
	$res = ["result" => "ok", "usernames" => []];
	foreach ($result as $row) 
		$res["usernames"][] = $row["sender"] == $username ? $row["reciever"] : $row["sender"];
	
    return $res;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    