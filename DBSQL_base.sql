CREATE DATABASE LudoVault;
USE LudoVault;

CREATE TABLE user (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(40) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
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
    name VARCHAR(20) NOT NULL
);

CREATE TABLE platform (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(20)
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

CREATE TABLE game_ranting (
	id INT AUTO_INCREMENT PRIMARY KEY,
    ranting DECIMAL(1,1) NOT NULL,
    game_id INT NOT NULL,
    user_id INT NOT NULL,
    comment TEXT,
    
	CONSTRAINT fk_game_ranting
    FOREIGN KEY(game_id)
    REFERENCES game(id),
	CONSTRAINT fk_user_ranting
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