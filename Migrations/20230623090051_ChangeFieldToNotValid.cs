using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarkovTestApp.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFieldToNotValid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsValid",
                table: "Employees",
                newName: "NotValid");

            migrationBuilder.RenameColumn(
                name: "IsValid",
                table: "Departments",
                newName: "NotValid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotValid",
                table: "Employees",
                newName: "IsValid");

            migrationBuilder.RenameColumn(
                name: "NotValid",
                table: "Departments",
                newName: "IsValid");
        }
    }
}
