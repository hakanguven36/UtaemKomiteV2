using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UtaemKomiteV2.Migrations
{
    public partial class mig00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dosya",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isim = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    uzantı = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    sysname = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    boyut = table.Column<double>(type: "float", nullable: false),
                    silindi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dosya", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kullar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    kulname = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    kulpass = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    hatirla = table.Column<bool>(type: "bit", nullable: false),
                    cerez = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true),
                    hatali = table.Column<int>(type: "int", nullable: false),
                    kilitliTarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    admin = table.Column<bool>(type: "bit", nullable: false),
                    adminCode = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullar", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dosya");

            migrationBuilder.DropTable(
                name: "Kullar");
        }
    }
}
