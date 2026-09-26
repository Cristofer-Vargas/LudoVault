using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LudoVault.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisherGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_developer_game_developer_developer_id",
                table: "developer_game");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_game_game_game_id",
                table: "developer_game");

            migrationBuilder.DropForeignKey(
                name: "FK_game_publisher",
                table: "game");

            migrationBuilder.DropIndex(
                name: "IX_game_publisher_id",
                table: "game");

            migrationBuilder.DropColumn(
                name: "publisher_id",
                table: "game");

            migrationBuilder.CreateTable(
                name: "publisher_game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    publisher_id = table.Column<int>(type: "int", nullable: false),
                    game_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_publisher_game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_publisher_game_game",
                        column: x => x.game_id,
                        principalTable: "game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_publisher_game_publisher",
                        column: x => x.publisher_id,
                        principalTable: "publisher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "publisher",
                columns: new[] { "Id", "fundation_at", "name" },
                values: new object[] { 4, new DateOnly(1992, 12, 31), "Take-Two Interactive" });

            migrationBuilder.InsertData(
                table: "publisher_game",
                columns: new[] { "Id", "game_id", "publisher_id" },
                values: new object[,]
                {
                    { 2, 1, 1 },
                    { 1, 1, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_publisher_game_game_id",
                table: "publisher_game",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "IX_publisher_game_publisher_id_game_id",
                table: "publisher_game",
                columns: new[] { "publisher_id", "game_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_developer_game_developer",
                table: "developer_game",
                column: "developer_id",
                principalTable: "developer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_developer_game_game",
                table: "developer_game",
                column: "game_id",
                principalTable: "game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_developer_game_developer",
                table: "developer_game");

            migrationBuilder.DropForeignKey(
                name: "FK_developer_game_game",
                table: "developer_game");

            migrationBuilder.DropTable(
                name: "publisher_game");

            migrationBuilder.DeleteData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "publisher_id",
                table: "game",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "game",
                keyColumn: "Id",
                keyValue: 1,
                column: "publisher_id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_game_publisher_id",
                table: "game",
                column: "publisher_id");

            migrationBuilder.AddForeignKey(
                name: "FK_developer_game_developer_developer_id",
                table: "developer_game",
                column: "developer_id",
                principalTable: "developer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_developer_game_game_game_id",
                table: "developer_game",
                column: "game_id",
                principalTable: "game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_game_publisher",
                table: "game",
                column: "publisher_id",
                principalTable: "publisher",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
