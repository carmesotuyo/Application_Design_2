using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogsApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RolModerador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Moderador",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Moderador",
                table: "Users");
        }
    }
}
