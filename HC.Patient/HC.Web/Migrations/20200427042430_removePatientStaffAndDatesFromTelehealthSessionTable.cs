using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class removePatientStaffAndDatesFromTelehealthSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthSessionDetails_Patients_PatientID",
                table: "TelehealthSessionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthSessionDetails_Staffs_StaffId",
                table: "TelehealthSessionDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TelehealthSessionDetails",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "TelehealthSessionDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "TelehealthSessionDetails",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "TelehealthSessionDetails",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthSessionDetails_Patients_PatientID",
                table: "TelehealthSessionDetails",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthSessionDetails_Staffs_StaffId",
                table: "TelehealthSessionDetails",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthSessionDetails_Patients_PatientID",
                table: "TelehealthSessionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthSessionDetails_Staffs_StaffId",
                table: "TelehealthSessionDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "TelehealthSessionDetails",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StaffId",
                table: "TelehealthSessionDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientID",
                table: "TelehealthSessionDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "TelehealthSessionDetails",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthSessionDetails_Patients_PatientID",
                table: "TelehealthSessionDetails",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthSessionDetails_Staffs_StaffId",
                table: "TelehealthSessionDetails",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
