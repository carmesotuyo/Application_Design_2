using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogsApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixSubComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isSubComment",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isSubComment",
                table: "Comments");
        }
    }
}
