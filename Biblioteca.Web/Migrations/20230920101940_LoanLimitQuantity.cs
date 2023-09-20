using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class LoanLimitQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoanLimitQuantity",
                table: "Books",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanLimitQuantity",
                table: "Books");
        }
    }
}
