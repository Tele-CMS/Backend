using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class nullablepatientidtoencounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounter_Patients_PatientID",
                table: "PatientEncounter");

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "PatientEncounter",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounter_Patients_PatientID",
                table: "PatientEncounter",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounter_Patients_PatientID",
                table: "PatientEncounter");

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "PatientEncounter",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounter_Patients_PatientID",
                table: "PatientEncounter",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
