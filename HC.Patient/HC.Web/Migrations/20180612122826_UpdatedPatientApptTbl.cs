using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatedPatientApptTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_Staffs_StaffID",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "PatientAppointment");

            migrationBuilder.RenameColumn(
                name: "StaffID",
                table: "PatientAppointment",
                newName: "PatientInsuranceId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAppointment_StaffID",
                table: "PatientAppointment",
                newName: "IX_PatientAppointment_PatientInsuranceId");

            migrationBuilder.AddColumn<int>(
                name: "AuthorizationId",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_AuthorizationId",
                table: "PatientAppointment",
                column: "AuthorizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_Authorization_AuthorizationId",
                table: "PatientAppointment",
                column: "AuthorizationId",
                principalTable: "Authorization",
                principalColumn: "AuthorizationId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_PatientInsuranceDetails_PatientInsuranceId",
                table: "PatientAppointment",
                column: "PatientInsuranceId",
                principalTable: "PatientInsuranceDetails",
                principalColumn: "PatientInsuranceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_Authorization_AuthorizationId",
                table: "PatientAppointment");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_PatientInsuranceDetails_PatientInsuranceId",
                table: "PatientAppointment");

            migrationBuilder.DropIndex(
                name: "IX_PatientAppointment_AuthorizationId",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "AuthorizationId",
                table: "PatientAppointment");

            migrationBuilder.RenameColumn(
                name: "PatientInsuranceId",
                table: "PatientAppointment",
                newName: "StaffID");

            migrationBuilder.RenameIndex(
                name: "IX_PatientAppointment_PatientInsuranceId",
                table: "PatientAppointment",
                newName: "IX_PatientAppointment_StaffID");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_Staffs_StaffID",
                table: "PatientAppointment",
                column: "StaffID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
