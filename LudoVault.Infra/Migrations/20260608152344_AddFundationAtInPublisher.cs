using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudoVault.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddFundationAtInPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fundation_at",
                table: "publisher",
                type: "DATETIME",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 1,
                column: "fundation_at",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 2,
                column: "fundation_at",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 3,
                column: "fundation_at",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fundation_at",
                table: "publisher");
        }
    }
}
