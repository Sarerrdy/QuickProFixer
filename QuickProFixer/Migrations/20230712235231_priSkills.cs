using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class priSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimarySkills",
                table: "FixerProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "OtherSkills",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OtherSkills",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimarySkills",
                table: "FixerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
