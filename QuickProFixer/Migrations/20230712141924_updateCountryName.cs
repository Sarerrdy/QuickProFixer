using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class updateCountryName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LgaId",
                table: "Requests",
                newName: "LGAId");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "Requests",
                newName: "StateName");

            migrationBuilder.RenameColumn(
                name: "Lga",
                table: "Requests",
                newName: "LGAName");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Requests",
                newName: "CountryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LGAId",
                table: "Requests",
                newName: "LgaId");

            migrationBuilder.RenameColumn(
                name: "StateName",
                table: "Requests",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "LGAName",
                table: "Requests",
                newName: "Lga");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "Requests",
                newName: "Country");
        }
    }
}
