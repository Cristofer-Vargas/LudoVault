CREATE DATABASE LudoVault;
USE LudoVault;

CREATE TABLE user (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(40) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    avatar_url VARCHAR(255) NOT NULL,
    passwordHash VARCHAR(255) NOT NULL,
    bio TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(name)
);

CREATE TABLE game (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    image_url VARCHAR(255) NOT NULL,
    description TEXT,
    publisher_id INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    INDEX(name),
    
    CONSTRAINT fk_game_publisher
    FOREIGN KEY(publisher_id)
    REFERENCES publisher(id)
);

CREATE TABLE publisher (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(60) NOT NULL
);

CREATE TABLE genre (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(40) UNIQUE NOT NULL
);

CREATE TABLE platform (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20) UNIQUE
);

CREATE TABLE game_genre (
	id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    genre_id INT NOT NULL,
    
	CONSTRAINT fk_game_genre
    FOREIGN KEY(game_id)
    REFERENCES game(id),
    CONSTRAINT fk_genre_genre
    FOREIGN KEY(genre_id)
    REFERENCES genre(id)
);

CREATE TABLE game_platform (
	id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    platform_id INT NOT NULL,
    
	CONSTRAINT fk_game_platform
    FOREIGN KEY(game_id)
    REFERENCES game(id),
    CONSTRAINT fk_platform_platform
    FOREIGN KEY(platform_id)
    REFERENCES platform(id)
);

CREATE TABLE game_rating (
	id INT AUTO_INCREMENT PRIMARY KEY,
    rating DECIMAL(2,1) NOT NULL DEFAULT 0,
    game_id INT NOT NULL,
    user_id INT NOT NULL,
    comment TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_game_rating
    FOREIGN KEY(game_id)
    REFERENCES game(id),
	CONSTRAINT fk_user_rating
    FOREIGN KEY(user_id)
    REFERENCES user(id)
);
    
CREATE TABLE user_list (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(60) NOT NULL,
    user_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_user_list_user
    FOREIGN KEY(user_id)
    REFERENCES user(id)
);

CREATE TABLE user_list_items (
	id INT AUTO_INCREMENT PRIMARY KEY,
    list_id INT NOT NULL,
    game_id INT NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_list_list
    FOREIGN KEY(list_id)
    REFERENCES user_list(id),
    CONSTRAINT fk_list_game
    FOREIGN KEY(game_id)
    REFERENCES game(id)
);

CREATE TABLE user_library (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(60) NOT NULL,
    user_id INT NOT NULL,
    
    CONSTRAINT fk_user_library_user
    FOREIGN KEY(user_id)
    REFERENCES user(id)
);

CREATE TABLE user_library_game (
	id INT AUTO_INCREMENT PRIMARY KEY,
    list_id INT NOT NULL,
    game_id INT NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

	CONSTRAINT fk_user_library_library
    FOREIGN KEY(list_id)
    REFERENCES user_library(id),
    CONSTRAINT fk_user_library_game
    FOREIGN KEY(game_id)
    REFERENCES game(id)
);


USE LudoVault;

# ==========================================================================================================================================

SELECT * FROM publisher;
SELECT * FROM platform;
SELECT * FROM genre;
SELECT * FROM rating;
SELECT * FROM game;
SELECT * FROM user;
SELECT * FROM user_library;
SELECT * FROM user_list;
SELECT * FROM user_list_items;