using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickProFixer.Migrations
{
    public partial class updateQuote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsQuoteAccepted",
                table: "Quotes",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsQuoteAccepted",
                table: "Quotes");
        }
    }
}
