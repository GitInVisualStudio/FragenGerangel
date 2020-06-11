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
	
	$result = $connection->select("user", ["username"], "`username` = '{$other_user}'");
	if (sizeof($result) == 0)
		throw new ObjectDoesntExistException();
	
	$result = $connection->select("user_is_friends", [], "`sender` = '{$username}' AND `reciever` = '{$other_user}'");
	if (sizeof($result) > 0)
		return ["result" => "ok"];
    
	$result = $connection->select("user_is_friends", [], "`sender` = '{$other_user}' AND `reciever` = '{$username}'");
	if (sizeof($result) == 0) {
		$data = [];
		$data[] = ["sender" => $username, "reciever" => $other_user, "accepted" => 0, "since" => "NOW()"];
		$connection->insert("user_is_friends", $data);
	} else {
		$connection->query("UPDATE user_is_friends SET `accepted` = 1 WHERE `sender` = '{$other_user}' AND `reciever` = '{$username}'");
	}
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
    