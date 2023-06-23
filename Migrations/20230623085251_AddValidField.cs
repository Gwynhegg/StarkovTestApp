using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarkovTestApp.Migrations
{
    /// <inheritdoc />
    public partial class AddValidField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Departments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Departments");
        }
    }
}
