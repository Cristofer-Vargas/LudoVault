using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LudoVault.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlterCreatedAtInTableGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "game",
                newName: "launched_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "launched_at",
                table: "game",
                newName: "created_at");
        }
    }
}
