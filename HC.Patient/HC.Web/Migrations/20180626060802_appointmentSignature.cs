using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class appointmentSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterSignature_User_UserId",
                table: "EncounterSignature");

            migrationBuilder.DropIndex(
                name: "IX_EncounterSignature_UserId",
                table: "EncounterSignature");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "EncounterSignature");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "EncounterSignature",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "EncounterSignature",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSignature_PatientId",
                table: "EncounterSignature",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSignature_StaffId",
                table: "EncounterSignature",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterSignature_Patients_PatientId",
                table: "EncounterSignature",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterSignature_Staffs_StaffId",
                table: "EncounterSignature",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncounterSignature_Patients_PatientId",
                table: "EncounterSignature");

            migrationBuilder.DropForeignKey(
                name: "FK_EncounterSignature_Staffs_StaffId",
                table: "EncounterSignature");

            migrationBuilder.DropIndex(
                name: "IX_EncounterSignature_PatientId",
                table: "EncounterSignature");

            migrationBuilder.DropIndex(
                name: "IX_EncounterSignature_StaffId",
                table: "EncounterSignature");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "EncounterSignature");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "EncounterSignature");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "EncounterSignature",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EncounterSignature_UserId",
                table: "EncounterSignature",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EncounterSignature_User_UserId",
                table: "EncounterSignature",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
