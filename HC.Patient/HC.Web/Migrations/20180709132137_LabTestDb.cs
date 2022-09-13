using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class LabTestDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_MasterLonic_LonicCodeID",
                table: "PatientLabTest");

            migrationBuilder.RenameColumn(
                name: "LonicCodeID",
                table: "PatientLabTest",
                newName: "LoincCodeID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_LonicCodeID",
                table: "PatientLabTest",
                newName: "IX_PatientLabTest_LoincCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_MasterLonic_LoincCodeID",
                table: "PatientLabTest",
                column: "LoincCodeID",
                principalTable: "MasterLonic",
                principalColumn: "LonicID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLabTest_MasterLonic_LoincCodeID",
                table: "PatientLabTest");

            migrationBuilder.RenameColumn(
                name: "LoincCodeID",
                table: "PatientLabTest",
                newName: "LonicCodeID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientLabTest_LoincCodeID",
                table: "PatientLabTest",
                newName: "IX_PatientLabTest_LonicCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLabTest_MasterLonic_LonicCodeID",
                table: "PatientLabTest",
                column: "LonicCodeID",
                principalTable: "MasterLonic",
                principalColumn: "LonicID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
