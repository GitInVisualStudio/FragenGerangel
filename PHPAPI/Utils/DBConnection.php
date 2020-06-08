<?php
class DBConnection
{
    private $connection = null;
    
    function __construct(string $host, string $user, string $password, string $database="") {
        if ($database !== "")
            $this->connection = new mysqli($host, $user, $password, $database);
        else
            $this->connection = new mysqli($host, $user, $password);
    }
    
    function query(string $query) : array {
        $res = $this->connection->query($query);
        if ($res === false)
            throw new Exception("Query {$query} failed with error: {$this->connection->error}");
        else if ($res === true)
            return [];
        return $res->fetch_all(MYSQLI_ASSOC);
    }
    
    function select(string $table, array $columns, string $where = "", string $order_by = "", string $limit = "") : array {
        $query = "SELECT ";
        if (empty($columns))
            $query .= "* ";
        else 
            $query .= $this->makeArraySqlReady($columns, "`") . " ";
        
        $query .= "FROM `{$table}` ";
        
        if (!empty($where))
            $query .= "WHERE {$where} ";
        
        if (!empty($order_by))
            $query .= "ORDER BY {$order_by} ";
        
        if (!empty($limit))
            $query .= "LIMIT {$limit}";
        
        return $this->query($query);
    }

    function insert(string $table, array $rows, string $extras = "") : array {
        if (empty($rows))
            return [];
        $cols = array_keys($rows[0]);
        $colsStr = "(" . $this->makeArraySqlReady($cols, "`") . ")";
        $rowsStr = $this->makeMultidimArraySqlReady($rows);
        $query = "INSERT INTO `{$table}` {$colsStr} VALUES {$rowsStr} {$extras}";
        $this->query($query);
        $start_id = $this->connection->insert_id;
        $end_id = $start_id + sizeof($rows) - 1;
        return range($start_id, $end_id);
    }
        
    private function makeArraySqlReady(array $arr, string $escape_char = "") : string {
        foreach ($arr as &$item) {
			if ($item != "NOW()")
				$item = $escape_char . $this->connection->real_escape_string($item) . $escape_char;
        }
        $res = implode(", ", $arr);
        return $res;
    }
    
    private function makeMultidimArraySqlReady(array $arr) : string {
        $res = [];
        foreach ($arr as $row)
            $res[] = "(" . $this->makeArraySqlReady($row, "'") . ")";

        return implode(", ", $res);
    }
    
    function realEscapeString(string $str) : string {
        return $this->connection->real_escape_string($str);
    }

    function lastInsertID() : int {
        return $this->connection->insert_id;
    }
}

