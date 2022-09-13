using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class appointmentStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PatientAppointment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientAppointment_StatusId",
                table: "PatientAppointment",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_GlobalCode_StatusId",
                table: "PatientAppointment",
                column: "StatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_GlobalCode_StatusId",
                table: "PatientAppointment");

            migrationBuilder.DropIndex(
                name: "IX_PatientAppointment_StatusId",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PatientAppointment");
        }
    }
}
