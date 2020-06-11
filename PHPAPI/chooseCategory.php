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

function main(array $post) : array {
    $connection = Globals::getDBConnection();
    
    if (!(array_key_exists("auth", $post) && array_key_exists("gameID", $post) && array_key_exists("category", $post)))
        throw new MissingParameterException();
    
	$username = Globals::getUsernameFromAuth($connection, $post["auth"]);
    $game_id = $connection->realEscapeString($post["gameID"]);
	$category_id = $connection->realEscapeString($post["category"]);
	
	if ($category_id < 1 || $category_id > 3)
		throw new WrongFormatException();
	
	$result = $connection->query("SELECT r.id AS id, r.cat_1 AS cat_1, r.cat_2 AS cat_2, r.cat_3 AS cat_3 FROM `round` r INNER JOIN game g ON r.game = g.id WHERE g.id = '{$game_id}' AND r.category IS NULL AND ((g.player_1 = '{$username}' AND r.`order` % 2 = 0) OR (g.player_2 = '{$username}' AND r.`order` % 2 = 1))");
	if (sizeof($result) == 0)
		throw new InsufficientPermissionsException();
	
	$result = $result[0];
	$round_id = $result["id"];
	$categories = [1 => $result["cat_1"], 2 => $result["cat_2"], 3 => $result["cat_3"]];
	$connection->query("UPDATE `round` SET category = '{$category_id}' WHERE `id` = '{$round_id}'");
	$category = $categories[$category_id];
	$result = $connection->select("question", [], "category = '{$category}'");
	$question_ids = [];
	foreach ($result as $row)
		$question_ids[] = $row["id"];
		
	$data = [];
	for ($i = 1; $i < 4; $i++) {
		$index = random_int(0, sizeof($question_ids) - 1);
		$question_id = Globals::arrayPop($question_ids, $index);
		$data[] = ["round" => $round_id, "order" => $i, "question" => $question_id];
	}
	$connection->insert("question_answer", $data);
	
	$result = ["result" => "ok"];
    return $result;
}

try {
    $post = Globals::parseJson(utf8_encode(file_get_contents('php://input')));
    $array = main($post);
} catch (Exception $e) {
    $array = ["result" => "error", "error_message" => $e->getMessage(), "error_code" => $e->getCode()];
}

echo json_encode($array);
    