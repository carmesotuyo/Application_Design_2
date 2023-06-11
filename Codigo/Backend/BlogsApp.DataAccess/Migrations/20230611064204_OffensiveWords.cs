using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogsApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class OffensiveWords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasContentToReview",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OffensiveWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffensiveWords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OffensiveWords");

            migrationBuilder.DropColumn(
                name: "HasContentToReview",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Articles");
        }
    }
}
