using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class PriceRate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FixingRateType",
                table: "FixerProfiles",
                newName: "PriceRateTypeId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Category",
                newName: "ServiceTypeName");

            migrationBuilder.AddColumn<string>(
                name: "PriceRateName",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceRateName",
                table: "FixerProfiles");

            migrationBuilder.RenameColumn(
                name: "PriceRateTypeId",
                table: "FixerProfiles",
                newName: "FixingRateType");

            migrationBuilder.RenameColumn(
                name: "ServiceTypeName",
                table: "Category",
                newName: "Name");
        }
    }
}
