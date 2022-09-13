using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedEncounterSignatureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EncounterSignature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClinicianSign = table.Column<byte[]>(nullable: true),
                    ClinicianSignDate = table.Column<DateTime>(nullable: true),
                    GuardianName = table.Column<string>(nullable: true),
                    GuardianSign = table.Column<byte[]>(nullable: true),
                    GuardianSignDate = table.Column<DateTime>(nullable: true),
                    PatientSign = table.Column<byte[]>(nullable: true),
                    PatientSignDate = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterSignature", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EncounterSignature_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSignature_UserId",
                table: "EncounterSignature",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncounterSignature");
        }
    }
}
