using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class AddHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendsHistory");

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "Lends",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DevolutionDate",
                table: "Lends",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookTitle",
                table: "Lends");

            migrationBuilder.DropColumn(
                name: "DevolutionDate",
                table: "Lends");

            migrationBuilder.CreateTable(
                name: "LendsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LentBooks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnedBooks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendsHistory", x => x.Id);
                });
        }
    }
}
