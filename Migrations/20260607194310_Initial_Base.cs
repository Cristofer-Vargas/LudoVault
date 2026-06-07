using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LudoVault.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Base : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "platform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_platform", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "publisher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publisher", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar_url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bio = table.Column<string>(type: "varchar(1200)", maxLength: 1200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password_hash = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    image_url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    publisher_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_publisher",
                        column: x => x.publisher_id,
                        principalTable: "publisher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_list",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_list", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_list_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "game_genre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    game_id = table.Column<int>(type: "int", nullable: false),
                    genre_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_genre", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_genre_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_genre_genre",
                        column: x => x.genre_id,
                        principalTable: "genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "game_platform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    game_id = table.Column<int>(type: "int", nullable: false),
                    platform_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_platform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_game_platform_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_game_platform_platform",
                        column: x => x.platform_id,
                        principalTable: "platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    rating = table.Column<decimal>(type: "decimal(2,1)", precision: 2, scale: 1, nullable: false),
                    comment = table.Column<string>(type: "varchar(1200)", maxLength: 1200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    game_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rating_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rating_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_library",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    game_id = table.Column<int>(type: "int", nullable: false),
                    added_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_library", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_library_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_library_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_list_game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    list_id = table.Column<int>(type: "int", nullable: false),
                    game_id = table.Column<int>(type: "int", nullable: false),
                    added_at = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_list_game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_list_item_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_list_item_list",
                        column: x => x.list_id,
                        principalTable: "user_list",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "genre",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "Ação" },
                    { 2, "Aventura" },
                    { 3, "RPG" },
                    { 4, "Mundo Aberto" }
                });

            migrationBuilder.InsertData(
                table: "platform",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "PlayStation 4" },
                    { 2, "Xbox One" },
                    { 3, "PC" }
                });

            migrationBuilder.InsertData(
                table: "publisher",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { 1, "Rockstar Games" },
                    { 2, "Nintendo" },
                    { 3, "Sony Interactive" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "avatar_url", "bio", "created_at", "email", "name", "password_hash" },
                values: new object[] { 1, "/uploads/users/default-image.webp", "Bem vindos ao meu perfil!", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "cristof@gmail.com", "Cristofer", "$2a$12$Ih3ZGAGY3KX9AQ6hJFfhL.NdBsGN0TonaaIPigkVBpb.VDN9aXuj2" });

            migrationBuilder.InsertData(
                table: "game",
                columns: new[] { "Id", "created_at", "description", "image_url", "name", "publisher_id" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Estados Unidos, 1899. Arthur Morgan e a gangue Van der Linde são forçados a fugir. Com agentes federais e os melhores caçadores de recompensas no seu encalço, a gangue precisa roubar, assaltar e lutar para sobreviver no impiedoso coração dos Estados Unidos. Conforme divisões internas profundas ameaçam despedaçar a gangue, Arthur deve fazer uma escolha entre os seus próprios ideais e a lealdade à gangue que o criou.", "/uploads/games/default-image.webp", "Red Dead Redemption 2", 1 });

            migrationBuilder.InsertData(
                table: "user_list",
                columns: new[] { "Id", "created_at", "name", "user_id" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meus Jogos Favoritos", 1 });

            migrationBuilder.InsertData(
                table: "game_genre",
                columns: new[] { "Id", "game_id", "genre_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "game_platform",
                columns: new[] { "Id", "game_id", "platform_id" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 1, 2 },
                    { 3, 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "rating",
                columns: new[] { "Id", "comment", "created_at", "game_id", "rating", "user_id" },
                values: new object[] { 1, "Uma verdadeira obra prima dos videogames!", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 5.0m, 1 });

            migrationBuilder.InsertData(
                table: "user_library",
                columns: new[] { "Id", "added_at", "game_id", "user_id" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.InsertData(
                table: "user_list_game",
                columns: new[] { "Id", "added_at", "game_id", "list_id" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_game_publisher_id",
                table: "game",
                column: "publisher_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_genre_game_id_genre_id",
                table: "game_genre",
                columns: new[] { "game_id", "genre_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_game_genre_genre_id",
                table: "game_genre",
                column: "genre_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_platform_game_id_platform_id",
                table: "game_platform",
                columns: new[] { "game_id", "platform_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_game_platform_platform_id",
                table: "game_platform",
                column: "platform_id");

            migrationBuilder.CreateIndex(
                name: "IX_rating_game_id",
                table: "rating",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_rating_user_id",
                table: "rating",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_library_game_id",
                table: "user_library",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_library_user_id_game_id",
                table: "user_library",
                columns: new[] { "user_id", "game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_list_user_id",
                table: "user_list",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_list_game_game_id",
                table: "user_list_game",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_list_game_list_id",
                table: "user_list_game",
                column: "list_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_genre");

            migrationBuilder.DropTable(
                name: "game_platform");

            migrationBuilder.DropTable(
                name: "rating");

            migrationBuilder.DropTable(
                name: "user_library");

            migrationBuilder.DropTable(
                name: "user_list_game");

            migrationBuilder.DropTable(
                name: "genre");

            migrationBuilder.DropTable(
                name: "platform");

            migrationBuilder.DropTable(
                name: "game");

            migrationBuilder.DropTable(
                name: "user_list");

            migrationBuilder.DropTable(
                name: "publisher");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
