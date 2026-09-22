using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LudoVault.Migrations
{
    /// <inheritdoc />
    public partial class AddDeveloperAndDeveloperGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "developer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    fundation_at = table.Column<DateOnly>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "developer_game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    developer_id = table.Column<int>(type: "int", nullable: false),
                    game_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developer_game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_developer_game_developer_developer_id",
                        column: x => x.developer_id,
                        principalTable: "developer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_developer_game_game_game_id",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "developer",
                columns: new[] { "Id", "fundation_at", "name" },
                values: new object[,]
                {
                    { 1, new DateOnly(2001, 12, 31), "Rockstar North" },
                    { 2, new DateOnly(1998, 12, 1), "Rockstar Games" }
                });

            migrationBuilder.InsertData(
                table: "developer_game",
                columns: new[] { "Id", "developer_id", "game_id" },
                values: new object[] { 1, 2, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_developer_game_developer_id",
                table: "developer_game",
                column: "developer_id");

            migrationBuilder.CreateIndex(
                name: "IX_developer_game_game_id_developer_id",
                table: "developer_game",
                columns: new[] { "game_id", "developer_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "developer_game");

            migrationBuilder.DropTable(
                name: "developer");
        }
    }
}
