using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModernWPFApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benutzer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalLoginTime = table.Column<TimeSpan>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benutzer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kunden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirmenName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Ansprechpartner = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Strasse = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PLZ = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Ort = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ErstelltAm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunden", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Baustellen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Strasse = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PLZ = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Ort = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    KundeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDatum = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baustellen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Baustellen_Kunden_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunden",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Aufmasse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nummer = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Titel = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    BaustelleId = table.Column<int>(type: "INTEGER", nullable: false),
                    AufmassAm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Notizen = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aufmasse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aufmasse_Baustellen_BaustelleId",
                        column: x => x.BaustelleId,
                        principalTable: "Baustellen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AufmassPositionen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AufmassId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Beschreibung = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Länge = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Breite = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Höhe = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Std = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Stück = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Lfm = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Einzelpreis = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Gesamt = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AufmassPositionen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AufmassPositionen_Aufmasse_AufmassId",
                        column: x => x.AufmassId,
                        principalTable: "Aufmasse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aufmasse_BaustelleId",
                table: "Aufmasse",
                column: "BaustelleId");

            migrationBuilder.CreateIndex(
                name: "IX_AufmassPositionen_AufmassId",
                table: "AufmassPositionen",
                column: "AufmassId");

            migrationBuilder.CreateIndex(
                name: "IX_Baustellen_KundeId",
                table: "Baustellen",
                column: "KundeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AufmassPositionen");

            migrationBuilder.DropTable(
                name: "Benutzer");

            migrationBuilder.DropTable(
                name: "Aufmasse");

            migrationBuilder.DropTable(
                name: "Baustellen");

            migrationBuilder.DropTable(
                name: "Kunden");
        }
    }
}
