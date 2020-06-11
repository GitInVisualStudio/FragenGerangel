SELECT g.id AS game_id, r.id AS round_id FROM question_answer q INNER JOIN `round` r ON q.`round` = r.id INNER JOIN game g ON r.game_id = g.id WHERE q.id = 1;
SELECT COUNT(*) FROM game g INNER JOIN `round` r ON r.game_id = g.id WHERE g.id = 3;
SELECT COUNT(*) FROM `round` r INNER JOIN question_answer q ON q.`round` = r.id WHERE r.id = 5 AND (answer_player_1 IS NULL OR answer_player_2 IS NULL);