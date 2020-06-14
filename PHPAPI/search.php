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
    
    if (!(array_key_exists("auth", $post) && array_key_exists("query", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$other_user = $connection->realEscapeString($post["query"]);
	if (empty($other_user))
		return ["result" => "ok", "usernames" => []];
	
	$result = $connection->query("SELECT username FROM user WHERE username LIKE '%{$other_user}%'");
	$res = ["result" => "ok", "usernames" => []];
	foreach ($result as $row) 
		if ($row["username"] != $username)
			$res["usernames"][] = $row["username"];
	
    return $res;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    