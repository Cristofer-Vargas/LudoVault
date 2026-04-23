CREATE DATABASE LudoVault;
USE LudoVault;

CREATE TABLE user (
	id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(40) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
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
    name VARCHAR(20) NOT NULL
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

USE LudoVault;

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


