using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class AddedGenreBookRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Books",
                newName: "GenreName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenreName",
                table: "Books",
                newName: "Genre");
        }
    }
}
