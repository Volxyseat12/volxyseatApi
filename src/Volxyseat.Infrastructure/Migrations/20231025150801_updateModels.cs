using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Volxyseat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Subscriptions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Clients",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "CPF",
                table: "Clients",
                newName: "CompanyName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Subscriptions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Clients",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Clients",
                newName: "CPF");
        }
    }
}
