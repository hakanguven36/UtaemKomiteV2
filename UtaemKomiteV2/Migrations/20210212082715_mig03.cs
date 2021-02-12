using Microsoft.EntityFrameworkCore.Migrations;

namespace UtaemKomiteV2.Migrations
{
    public partial class mig03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "Dosya",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "Dosya");
        }
    }
}
