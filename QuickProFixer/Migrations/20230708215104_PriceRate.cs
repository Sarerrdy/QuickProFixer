using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class PriceRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "FixerProfiles");

            migrationBuilder.DropColumn(
                name: "IsEmployee",
                table: "FixerProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "CACNumber",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FixingRateType",
                table: "FixerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "priceRateTypes",
                columns: table => new
                {
                    PriceRateTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceRateName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priceRateTypes", x => x.PriceRateTypeId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "priceRateTypes");

            migrationBuilder.DropColumn(
                name: "FixingRateType",
                table: "FixerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "CACNumber",
                table: "FixerProfiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployee",
                table: "FixerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
