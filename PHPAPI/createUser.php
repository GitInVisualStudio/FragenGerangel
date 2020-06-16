<?php

require_once("Utils/DBConnection.php");

require_once("Utils/Globals.php");

require_once("Exceptions/MissingParameterException.php");

require_once("Exceptions/WrongCredentialsException.php");

require_once("Exceptions/WrongFormatException.php");

require_once("Exceptions/IllegalOperationException.php");


set_error_handler(function($errno, $errstr, $errfile, $errline, $errcontext) {

    // error was suppressed with the @-operator

    if (0 === error_reporting()) {

        return false;

    }



    throw new Exception($errstr, $errno);

});



function start(array $post) : array {

    $connection = Globals::getDBConnection();

    

    if (!(array_key_exists("username", $post) && array_key_exists("password", $post)))

        throw new MissingParameterException();



    $username = $connection->realEscapeString($post["username"]);

    $password = $connection->realEscapeString($post["password"]);

	$salt = random_int(100000, 999999);

    $hash = Globals::hashAndSalt($password, $salt);

	Globals::logStr("{$username} tried to create user");

	$result = $connection->select("user", ["username"], "`username` = '{$username}'");

	if (sizeof($result) != 0)

		throw new IllegalOperationException();

	

	$data = [];

	$data[] = ["username" => $username, "password" => $hash, "salt" => $salt];

    $result = $connection->insert("user", $data);

    $result = ["result" => "ok"];

    return $result;

}



try {

    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
	
    $array = start($post);

} catch (Exception $e) {

    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
	Globals::logStr("Exception! " . json_encode($array));
}



echo json_encode($array);
Globals::logStr("createUser.php finished");
    