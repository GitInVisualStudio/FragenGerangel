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
    
    if (!(array_key_exists("auth", $post) && array_key_exists("questionID", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
	$question_id = $connection->realEscapeString($post["questionID"]);
	
	$result = $connection->select("question", [], "`id` = '{$question_id}'");
	$question = [];
	$row = $result[0];
	$question["category"] = $row["category"];
	$question["question"] = $row["question"];
	$question["correctAnswer"] = $row["correct_answer"];
	$question["wrongAnswer1"] = $row["wrong_answer_1"];
	$question["wrongAnswer2"] = $row["wrong_answer_2"];
	$question["wrongAnswer3"] = $row["wrong_answer_3"];
	
	$result = ["result" => "ok", "question" => $question];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = start($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    