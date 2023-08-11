using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "AuthorImage",
                table: "Authors");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverId",
                table: "Rentals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AuthorImageId",
                table: "Authors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverId",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "AuthorImageId",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Rentals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorImage",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
