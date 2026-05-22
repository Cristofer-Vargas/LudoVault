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
    REFERENCES game(id)
    ON DELETE CASCADE,
    
    CONSTRAINT fk_genre_genre
    FOREIGN KEY(genre_id)
    REFERENCES genre(id)
    ON DELETE CASCADE
);

CREATE TABLE game_platform (
	id INT AUTO_INCREMENT PRIMARY KEY,
    game_id INT NOT NULL,
    platform_id INT NOT NULL,
    
	CONSTRAINT fk_game_platform
    FOREIGN KEY(game_id)
    REFERENCES game(id)
    ON DELETE CASCADE,
    
    CONSTRAINT fk_platform_platform
    FOREIGN KEY(platform_id)
    REFERENCES platform(id)
    ON DELETE CASCADE
);

CREATE TABLE rating (
	id INT AUTO_INCREMENT PRIMARY KEY,
    rating DECIMAL(2,1) NOT NULL DEFAULT 0,
    game_id INT NOT NULL,
    user_id INT NOT NULL,
    comment TEXT,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_rating_game
    FOREIGN KEY(game_id)
    REFERENCES game(id)
    ON DELETE CASCADE,
    
	CONSTRAINT fk_rating_user
    FOREIGN KEY(user_id)
    REFERENCES user(id)
    ON DELETE CASCADE
);
    
CREATE TABLE user_list (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(60) NOT NULL,
    user_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_user_list_user
    FOREIGN KEY(user_id)
    REFERENCES user(id)
    ON DELETE CASCADE
);

CREATE TABLE user_list_items (
	id INT AUTO_INCREMENT PRIMARY KEY,
    list_id INT NOT NULL,
    game_id INT NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
	CONSTRAINT fk_list_list
    FOREIGN KEY(list_id)
    REFERENCES user_list(id)
    ON DELETE CASCADE,
    
    CONSTRAINT fk_list_game
    FOREIGN KEY(game_id)
    REFERENCES game(id)
    ON DELETE CASCADE
);


CREATE TABLE user_library (
	id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    game_id INT NOT NULL,
	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

	CONSTRAINT fk_user_library_library 
    FOREIGN KEY(user_id)
    REFERENCES user(id)
    ON DELETE CASCADE,
    
    CONSTRAINT fk_user_library_game
    FOREIGN KEY(game_id)
    REFERENCES game(id)
    ON DELETE CASCADE
);

# ==========================================================================================================================================

SELECT * FROM user;
SELECT * FROM game;
SELECT * FROM publisher;
SELECT * FROM rating;
SELECT * FROM user_library;
SELECT * FROM user_list;
SELECT * FROM user_list_items;

# ---
INSERT INTO publisher (name) VALUES 
("Rockstar"),("Square Enix"),("Ubsoft"),("Mojang"),("miHoYo"),("Blizzard Entertainment"),("Riot Games");

# ---
INSERT INTO platform (name) VALUES ("PC"),("Xbox Series S"),("Mobile"),("Nintendo switch"),('PlayStation 5'),('PlayStation 4'),('PlayStation 3'),
('Xbox Series X/S'),('Xbox One'),('Xbox 360'),('Nintendo Switch'),('Nintendo Wii U'),('Nintendo 3DS'),('Android'),('iOS'),('Linux'),('macOS'),
('Web Browser'),('PlayStation Vita'),('Nintendo DS'),('PlayStation 2'),('GameCube'),('Dreamcast'),('Xbox (Original)');

# ---
INSERT INTO genre (name) VALUES 
('Ação'),('Aventura'),('RPG (Role-Playing Game)'),('FPS (First-Person Shooter)'),('TPS (Third-Person Shooter)'),('Estratégia'),('Simulação'),('Esporte'),
('Corrida'),('Luta'),('Terror/Survival Horror'),('Plataforma'),('Puzzle'),('MMORPG'),('Roguelike'),('Metroidvania'),('Hack and Slash'),('Stealth'),('Visual Novel'),
('Battle Royale'),('Mundo Aberto'),('Indie'),('Casual'),('Musical/Ritmo');