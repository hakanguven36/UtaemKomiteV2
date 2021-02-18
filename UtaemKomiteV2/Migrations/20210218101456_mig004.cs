using Microsoft.EntityFrameworkCore.Migrations;

namespace UtaemKomiteV2.Migrations
{
    public partial class mig004 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tur",
                table: "Dosya",
                newName: "turID");

            migrationBuilder.CreateTable(
                name: "Tur",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isim = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tur", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dosya_turID",
                table: "Dosya",
                column: "turID");

            migrationBuilder.AddForeignKey(
                name: "FK_Dosya_Tur_turID",
                table: "Dosya",
                column: "turID",
                principalTable: "Tur",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dosya_Tur_turID",
                table: "Dosya");

            migrationBuilder.DropTable(
                name: "Tur");

            migrationBuilder.DropIndex(
                name: "IX_Dosya_turID",
                table: "Dosya");

            migrationBuilder.RenameColumn(
                name: "turID",
                table: "Dosya",
                newName: "tur");
        }
    }
}
