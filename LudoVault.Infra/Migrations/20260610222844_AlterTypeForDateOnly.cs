using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudoVault.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlterTypeForDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "fundation_at",
                table: "publisher",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "launched_at",
                table: "game",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");

            migrationBuilder.UpdateData(
                table: "game",
                keyColumn: "Id",
                keyValue: 1,
                column: "launched_at",
                value: new DateOnly(2018, 10, 26));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 1,
                column: "fundation_at",
                value: new DateOnly(1998, 12, 1));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 2,
                column: "fundation_at",
                value: new DateOnly(1889, 9, 1));

            migrationBuilder.UpdateData(
                table: "publisher",
                keyColumn: "Id",
                keyValue: 3,
                column: "fundation_at",
                value: new DateOnly(1993, 11, 16));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "fundation_at",
                table: "publisher",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "launched_at",
                table: "game",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "DATE");

            migrationBuilder.UpdateData(
                table: "game",
                keyColumn: "Id",
                keyValue: 1,
                column: "launched_at",
                value: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
    }
}
