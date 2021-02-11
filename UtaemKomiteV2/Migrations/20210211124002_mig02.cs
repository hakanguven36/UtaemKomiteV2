using Microsoft.EntityFrameworkCore.Migrations;

namespace UtaemKomiteV2.Migrations
{
    public partial class mig02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "kulName",
                table: "Dosya",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "kulName",
                table: "Dosya");
        }
    }
}
