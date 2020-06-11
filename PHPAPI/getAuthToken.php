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
    
    if (!(array_key_exists("username", $post) && array_key_exists("password", $post)))
        throw new MissingParameterException();
    
    $username = $connection->realEscapeString($post["username"]);
    $password = $connection->realEscapeString($post["password"]);
    $result = $connection->select("user", ["salt"], "`username` = '{$username}'");
    if (empty($result))
        throw new WrongCredentialsException();
    $salt = $result[0]["salt"];
    $hash = Globals::hashAndSalt($password, $salt);
    $result = $connection->select("user", ["username"], "`username` = '{$username}' AND `password` = '{$hash}'");
    if (sizeof($result) === 0)
        throw new WrongCredentialsException();
    
    $user_id = $result[0]["username"];
    $dt = Globals::dateTimeNow();
    do {
        $token = hash("sha256", $username . time());
        $result = $connection->select("user", ["username"], "`auth` = '{$token}'");
    } while(sizeof($result) !== 0);
    $connection->query("UPDATE user SET `auth` = '{$token}', `auth_since` = '{$dt}' WHERE `username` = '{$user_id}'");
    $result = ["result" => "ok", "token" => $token];
    
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    