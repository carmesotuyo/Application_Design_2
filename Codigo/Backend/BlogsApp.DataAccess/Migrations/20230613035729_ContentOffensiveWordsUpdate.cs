using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogsApp.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ContentOffensiveWordsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleOffensiveWord",
                columns: table => new
                {
                    OffensiveWordsId = table.Column<int>(type: "int", nullable: false),
                    articlesContainingWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleOffensiveWord", x => new { x.OffensiveWordsId, x.articlesContainingWordId });
                    table.ForeignKey(
                        name: "FK_ArticleOffensiveWord_Articles_articlesContainingWordId",
                        column: x => x.articlesContainingWordId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleOffensiveWord_OffensiveWords_OffensiveWordsId",
                        column: x => x.OffensiveWordsId,
                        principalTable: "OffensiveWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentOffensiveWord",
                columns: table => new
                {
                    OffensiveWordsId = table.Column<int>(type: "int", nullable: false),
                    commentsContainingWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentOffensiveWord", x => new { x.OffensiveWordsId, x.commentsContainingWordId });
                    table.ForeignKey(
                        name: "FK_CommentOffensiveWord_Comments_commentsContainingWordId",
                        column: x => x.commentsContainingWordId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentOffensiveWord_OffensiveWords_OffensiveWordsId",
                        column: x => x.OffensiveWordsId,
                        principalTable: "OffensiveWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleOffensiveWord_articlesContainingWordId",
                table: "ArticleOffensiveWord",
                column: "articlesContainingWordId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentOffensiveWord_commentsContainingWordId",
                table: "CommentOffensiveWord",
                column: "commentsContainingWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleOffensiveWord");

            migrationBuilder.DropTable(
                name: "CommentOffensiveWord");
        }
    }
}
