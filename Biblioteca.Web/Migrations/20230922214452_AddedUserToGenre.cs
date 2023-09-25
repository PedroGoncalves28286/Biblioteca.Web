using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class AddedUserToGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Genres",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_UserId",
                table: "Genres",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_AspNetUsers_UserId",
                table: "Genres",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_AspNetUsers_UserId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_UserId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Genres");
        }
    }
}
