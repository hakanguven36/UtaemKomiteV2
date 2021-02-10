using Microsoft.EntityFrameworkCore.Migrations;

namespace UtaemKomiteV2.Migrations
{
    public partial class mig01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "tur",
                table: "Dosya",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tur",
                table: "Dosya");
        }
    }
}
