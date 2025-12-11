using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TopoCentras.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Klientai",
                columns: table => new
                {
                    KlientasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Pavadinimas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klientai", x => x.KlientasId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prekes",
                columns: table => new
                {
                    PrekeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Pavadinimas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gamintojas = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Kaina = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prekes", x => x.PrekeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Uzsakymai",
                columns: table => new
                {
                    UzsakymasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    KlientasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BendraUzsakymoSuma = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UzsakymoSukurimoData = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzsakymai", x => x.UzsakymasId);
                    table.ForeignKey(
                        name: "FK_Uzsakymai_Klientai_KlientasId",
                        column: x => x.KlientasId,
                        principalTable: "Klientai",
                        principalColumn: "KlientasId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UzsakymoPrekes",
                columns: table => new
                {
                    UzsakymasId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PrekeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Kiekis = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UzsakymoPrekes", x => new { x.UzsakymasId, x.PrekeId });
                    table.ForeignKey(
                        name: "FK_UzsakymoPrekes_Prekes_PrekeId",
                        column: x => x.PrekeId,
                        principalTable: "Prekes",
                        principalColumn: "PrekeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UzsakymoPrekes_Uzsakymai_UzsakymasId",
                        column: x => x.UzsakymasId,
                        principalTable: "Uzsakymai",
                        principalColumn: "UzsakymasId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Uzsakymai_KlientasId",
                table: "Uzsakymai",
                column: "KlientasId");

            migrationBuilder.CreateIndex(
                name: "IX_UzsakymoPrekes_PrekeId",
                table: "UzsakymoPrekes",
                column: "PrekeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UzsakymoPrekes");

            migrationBuilder.DropTable(
                name: "Prekes");

            migrationBuilder.DropTable(
                name: "Uzsakymai");

            migrationBuilder.DropTable(
                name: "Klientai");
        }
    }
}
