<?php
require_once("DBConnection.php");

class Globals
{
    const savePath = "saves/";
    private static $dbConnection = null;

    static function dateTimeNow() : string {
        return date("Y-m-d H:i:s", time());
    }
        
    static function dateDifference(string $date1, string $date2) : int {
        $t1 = strtotime($date1);
        $t2 = strtotime($date2);
        return abs($t1 - $t2);
    }

    static function cSharpDateTimeToPHPDateTime(string $dt) : string {
        $dt = str_replace("T", " ", $dt);
        $ms_position = strpos($dt, ".");
        if ($ms_position)
            $dt = substr($dt, 0, $ms_position);
        return $dt;
    }

    static function parseJson(string $json) : array {
        $json = str_replace("\\", "", utf8_decode($json));
        $json = str_replace("?", "", $json);
        $json = json_decode($json, true);
        if ($json === null)
            throw new WrongFormatException(); // json was invalid
        return $json;
    }

    static function hashAndSalt(string $password, int $salt) : string {
        return hash("sha256", $password . $salt);
    }

    static function intToTestType(int $t) : string {
        switch ($t) {
            case 0:
                return "exam";
                break;
            case 1:
                return "vocabulary_test";
                break;
            case 2:
                return "test";
                break;
        }
        return "";
    }

    static function testTypeToInt(string $type) : int {
        switch ($type) {
            case "exam":
                return 0;
                break;
            case "vocabulary_test":
                return 1;
                break;
            case "test":
                return 2;
                break;
        }
        return -1;
    }

    static function getDBConnection() : DBConnection {
        if (Globals::$dbConnection == null)
            Globals::$dbConnection = new DBConnection("127.0.0.1", "ni418681_1sql1", "fragengerangel", "ni418681_1sql1");
        return Globals::$dbConnection;
    }
	
	static function getUsernameFromAuth(DBConnection $connection, string $auth) : string {
		$auth = $connection->realEscapeString($auth);

		$result = $connection->select("user", ["username", "auth_since"], "`auth` = '{$auth}'");
		if (sizeof($result) === 0)
			throw new WrongCredentialsException(); // auth token doesnt exist

		$username = $result[0]["username"];
		$auth_since = $result[0]["auth_since"];
		$days = Globals::dateDifference($auth_since, Globals::dateTimeNow()) / 60 / 60 / 24;
		if ($days >= 1)
			throw new AuthTokenExpiredException(); // auth token expired
		
		return $username;
	}
	
	static function initializeNewRound(DBConnection $connection, int $game_id) : void {
		$result = $connection->select("round", ["order", "cat_1", "cat_2", "cat_3", "category"], "`game` = {$game_id}", "`order` DESC");
		if (sizeof($result) == 0)
			$order = 1;
		else
			$order = $result[0]["order"] + 1;
		
		$already_played_categories = [];
		foreach ($result as $row) {
			if ($row["category"] == null)
				break;
			$categories = [$row["cat_1"], $row["cat_2"], $row["cat_3"]];
			$category_index = $row["category"] - 1;
			$already_played_categories[] = $categories[$category_index];
		}
		
		$result = $connection->select("question` `q", ["category"], "(SELECT COUNT(*) FROM question qq WHERE q.category = qq.category) >= 3");
		if (sizeof($result) < 3)
			throw new ServerException();
		$all_categories = [];
		foreach ($result as $row)
			if (!in_array($row["category"], $already_played_categories))
				$all_categories[] = $row["category"];
			
		$categories = [];
		for ($i = 0; $i < 3; $i++){
			$index = random_int(0, sizeof($all_categories) - 1);
			$categories[$i] = Globals::arrayPop($all_categories, $index);
		}
		$data = [];
		$data[] = ["game" => $game_id, "order" => $order, "cat_1" => $categories[0], "cat_2" => $categories[1], "cat_3" => $categories[2]];
		$connection->insert("round", $data);
	}
	
	static function arrayPop(array &$arr, int $index) {
		$value = $arr[$index];
		unset($arr[$index]);
		return $value;
	}
}

