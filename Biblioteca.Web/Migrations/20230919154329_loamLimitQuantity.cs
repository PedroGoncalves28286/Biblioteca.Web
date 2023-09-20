using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class loamLimitQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "LendDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LendDetails");
        }
    }
}
