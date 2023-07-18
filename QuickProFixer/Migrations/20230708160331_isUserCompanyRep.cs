using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class isUserCompanyRep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUserCompanyRep",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUserCompanyRep",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "AspNetUsers");
        }
    }
}
