using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volxyseat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Clients",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "Nome");
        }
    }
}
