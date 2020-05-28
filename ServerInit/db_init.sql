-- Database Structure for FragenGerangel

CREATE DATABASE IF NOT EXISTS `FragenGerangel` CHARACTER SET utf8mb4;
USE FragenGerangel;

-- TABLE User

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`user` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`email` VARCHAR(64) NOT NULL,
	`username` VARCHAR(32) NOT NULL,
	`password` CHAR(64) NOT NULL,
	`salt` INT NOT NULL,
	`auth` CHAR(64) NULL,
	`auth_since` DATETIME NULL,
	`elo` INT NOT NULL DEFAULT 1500,
	PRIMARY KEY (`id`)
)
ENGINE=InnoDB;

-- TABLE Question

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`question` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`category` VARCHAR(128) NOT NULL,
	`question` VARCHAR(256) NOT NULL,
	`correct_answer` VARCHAR(256) NOT NULL,
	`wrong_answer_1` VARCHAR(256) NOT NULL,
	`wrong_answer_2` VARCHAR(256) NOT NULL,
	`wrong_answer_3` VARCHAR(256) NOT NULL,
	PRIMARY KEY (`id`)
)
ENGINE=InnoDB;

-- TABLE user_is_friends

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`user_is_friends` (
	`sender` BIGINT NOT NULL AUTO_INCREMENT,
	`reciever` BIGINT NOT NULL,
	`accepted` BOOLEAN,
	`since` DATETIME NOT NULL,
	PRIMARY KEY (`sender`, `reciever`),
	FOREIGN KEY (`sender`) REFERENCES `FragenGerangel`.`user`(`id`) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (`reciever`) REFERENCES `FragenGerangel`.`user`(`id`) ON UPDATE CASCADE ON DELETE CASCADE
)
ENGINE=InnoDB;

-- TABLE Game

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`Game` (
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`player_1` BIGINT NULL,
	`player_2` BIGINT NULL,
	`won_by` BIGINT NULL,
	PRIMARY KEY (`id`),
	FOREIGN KEY (`player_1`) REFERENCES `FragenGerangel`.`user`(`id`) ON UPDATE CASCADE ON DELETE SET NULL,
	FOREIGN KEY (`player_2`) REFERENCES `FragenGerangel`.`user`(`id`) ON UPDATE CASCADE ON DELETE SET NULL,
	FOREIGN KEY (`won_by`) REFERENCES `FragenGerangel`.`user`(`id`) ON UPDATE CASCADE ON DELETE SET NULL
)
ENGINE=InnoDB;

-- TABLE Round

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`Round` ( 
	`id` BIGINT NOT NULL AUTO_INCREMENT,
	`game_id` BIGINT NOT NULL,
	`order` TINYINT NOT NULL,
	`category` VARCHAR(128) NOT NULL,
	PRIMARY KEY (`id`)
)
ENGINE=InnoDB;

-- TABLE QuestionAnswer

CREATE TABLE IF NOT EXISTS `FragenGerangel`.`QuestionAnswer` (
	`round` BIGINT NOT NULL,
	`order` TINYINT NOT NULL,
	`question` BIGINT NOT NULL,
	`answer_player_1` TINYINT NULL,
	`answer_player_2` TINYINT NULL,
	PRIMARY KEY(`id`),
	FOREIGN KEY (`round`) REFERENCES `FragenGerangel`.`Round`(`id`) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY (`question`) REFERENCES `FragenGerangel`.`Question`(`id`) ON UPDATE CASCADE ON DELETE CASCADE,
)
ENGINE=InnoDB;