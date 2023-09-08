using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Biblioteca.Web.Migrations
{
    public partial class AddedLendModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropColumn(
                name: "Disable",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MembershipID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ActualReturnDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "RentalDuration",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ScheduleReturnDate",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Books",
                newName: "SelectedDate");

            migrationBuilder.CreateTable(
                name: "Lends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lends_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lends_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LendsDetailTemp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BookId = table.Column<int>(type: "int", nullable: true),
                    LendDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendsDetailTemp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendsDetailTemp_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LendsDetailTemp_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LendDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LendId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LendDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LendDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LendDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LendDetails_Lends_LendId",
                        column: x => x.LendId,
                        principalTable: "Lends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LendDetails_BookId",
                table: "LendDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LendDetails_LendId",
                table: "LendDetails",
                column: "LendId");

            migrationBuilder.CreateIndex(
                name: "IX_LendDetails_UserId",
                table: "LendDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lends_BookId",
                table: "Lends",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Lends_UserId",
                table: "Lends",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LendsDetailTemp_BookId",
                table: "LendsDetailTemp",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_LendsDetailTemp_UserId",
                table: "LendsDetailTemp",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LendDetails");

            migrationBuilder.DropTable(
                name: "LendsDetailTemp");

            migrationBuilder.DropTable(
                name: "Lends");

            migrationBuilder.RenameColumn(
                name: "SelectedDate",
                table: "Books",
                newName: "StartDate");

            migrationBuilder.AddColumn<bool>(
                name: "Disable",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MembershipID",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualReturnDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Availability",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalDuration",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleReturnDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeRateSixMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    ChargeRateTwelveMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignUpFee = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                table: "Memberships",
                column: "UserId");
        }
    }
}
