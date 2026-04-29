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

# ---

SELECT * FROM game_rating;

INSERT INTO game_rating (rating, game_id, user_id, comment) VALUES 
(4.2, 7, 1, "Joguinho de herói com poderzin Online. Viciante demais HAHAHA!");
INSERT INTO game_rating (rating, game_id, user_id, comment) VALUES 
(5, 4, 1, "Que jogasso maravilhoso senhoras e senhores. Recomendo demaaaais!!");
INSERT INTO game_rating (rating, game_id, user_id, comment) VALUES 
(4.5, 4, 18, "Jogo cawboyzeiro, top demais. No final ainda da pra trabalhar com a propria fazenda!");
INSERT INTO game_rating (rating, game_id, user_id, comment) VALUES 
(3, 4, 19, "Não achei tudo isso, jogo bem básico.");

# ---
SELECT * FROM user;

# ---
SELECT * FROM publisher;

SELECT * FROM publisher 
LEFT JOIN game 
ON publisher.id = game.id;

INSERT INTO publisher (name) VALUES ("Rockstar");
INSERT INTO publisher (name) VALUES ("Square Enix");
INSERT INTO publisher (name) VALUES ("Ubsoft");
INSERT INTO publisher (name) VALUES ("Mojang");
INSERT INTO publisher (name) VALUES ("miHoYo");
INSERT INTO publisher (name) VALUES ("Blizzard Entertainment");
INSERT INTO publisher (name) VALUES ("Riot Games");

# ---
DELETE FROM platform WHERE id = 13;
SELECT * FROM platform ORDER BY name;

INSERT INTO platform (name) VALUES ("PC");
INSERT INTO platform (name) VALUES ("Xbox Series S");
INSERT INTO platform (name) VALUES ("Mobile");
INSERT INTO platform (name) VALUES ("Nintendo switch");

INSERT INTO platform (name) VALUES 
('PlayStation 5'),
('PlayStation 4'),
('PlayStation 3'),
('Xbox Series X/S'),
('Xbox One'),
('Xbox 360'),
('Nintendo Switch'),
('Nintendo Wii U'),
('Nintendo 3DS'),
('Android'),
('iOS'),
('Linux'),
('macOS'),
('Web Browser'),
('PlayStation Vita'),
('Nintendo DS'),
('PlayStation 2'),
('GameCube'),
('Dreamcast'),
('Xbox (Original)');

# ---
SELECT * FROM genre;

INSERT INTO genre (name) VALUES 
('Ação'),('Aventura'),('RPG (Role-Playing Game)'),('FPS (First-Person Shooter)'),('TPS (Third-Person Shooter)'),('Estratégia'),('Simulação'),('Esporte'),
('Corrida'),('Luta'),('Terror/Survival Horror'),('Plataforma'),('Puzzle'),('MMORPG'),('Roguelike'),('Metroidvania'),('Hack and Slash'),('Stealth'),('Visual Novel'),
('Battle Royale'),('Mundo Aberto'),('Indie'),('Casual'),('Musical/Ritmo');

# ---
INSERT INTO game_genre (game_id, genre_id) VALUES
(3, 73), (3, 74), (3, 81), (3, 93), (3, 77);

# ---
SELECT * FROM game_platform;

INSERT INTO game_platform (game_id, platform_id) VALUES (3, 1);
INSERT INTO game_platform (game_id, platform_id) VALUES (4, 1);
INSERT INTO game_platform (game_id, platform_id) VALUES (5, 1);
INSERT INTO game_platform (game_id, platform_id) VALUES (5, 3);
INSERT INTO game_platform (game_id, platform_id) VALUES (5, 4);

SELECT g.name, p.name
FROM 
	game AS g, 
	platform AS p,
    game_platform AS gp
WHERE gp.game_id = g.id;

# ---
SELECT * FROM game;

INSERT INTO game (name, image_url, description, publisher_id) 
VALUES ("Grand Theft Auto V", "wwwroot/uploads/games/caminhoimagem.jpg", "GTAV é bom demaaaaais", 4);
INSERT INTO game (name, image_url, description, publisher_id) 
VALUES ("Read Dead Redemption 2", "wwwroot/uploads/games/caminhoimagem.jpg", "Jogo do Ano!!", 4);
INSERT INTO game (name, image_url, description, publisher_id) 
VALUES ("Genshin Impact", "wwwroot/uploads/games/caminhoimagem.jpg", "Joguinho Gasha!", 9);
INSERT INTO game (name, image_url, description, publisher_id) 
VALUES ("Minecraft", "wwwroot/uploads/games/caminhoimagem.jpg", "Jogo Quadrado", 7);

#---
SELECT * FROM user_list_items WHERE list_id = 1;

INSERT INTO user_list (name, user_id) VALUES("Meus Favoritos", 18);
INSERT INTO user_list (name, user_id) VALUES("Meus Favoritos", 1);
INSERT INTO user_list_items (list_id, game_id) VALUES(1, 4);
INSERT INTO user_list_items (list_id, game_id) VALUES(1, 7),(1, 3),(1, 6);
INSERT INTO user_list (name, user_id) VALUES("Zerados", 1);
INSERT INTO user_list_items (list_id, game_id) VALUES(2, 3), (2, 4), (2, 6);