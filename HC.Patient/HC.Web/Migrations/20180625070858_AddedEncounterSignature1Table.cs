using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedEncounterSignature1Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientEncounterId",
                table: "EncounterSignature",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSignature_PatientEncounterId",
                table: "EncounterSignature",
                column: "PatientEncounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterSignature_PatientEncounter_PatientEncounterId",
                table: "EncounterSignature",
                column: "PatientEncounterId",
                principalTable: "PatientEncounter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterSignature_PatientEncounter_PatientEncounterId",
                table: "EncounterSignature");

            migrationBuilder.DropIndex(
                name: "IX_EncounterSignature_PatientEncounterId",
                table: "EncounterSignature");

            migrationBuilder.DropColumn(
                name: "PatientEncounterId",
                table: "EncounterSignature");
        }
    }
}
