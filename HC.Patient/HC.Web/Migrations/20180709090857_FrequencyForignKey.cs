using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class FrequencyForignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PatientMedication_FrequencyID",
                table: "PatientMedication",
                column: "FrequencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientMedication_GlobalCode_FrequencyID",
                table: "PatientMedication",
                column: "FrequencyID",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientMedication_GlobalCode_FrequencyID",
                table: "PatientMedication");

            migrationBuilder.DropIndex(
                name: "IX_PatientMedication_FrequencyID",
                table: "PatientMedication");
        }
    }
}
