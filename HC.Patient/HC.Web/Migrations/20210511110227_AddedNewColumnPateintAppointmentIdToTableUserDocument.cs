using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnPateintAppointmentIdToTableUserDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientAppointmentId",
                table: "UserDocuments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_PatientAppointmentId",
                table: "UserDocuments",
                column: "PatientAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_PatientAppointment_PatientAppointmentId",
                table: "UserDocuments",
                column: "PatientAppointmentId",
                principalTable: "PatientAppointment",
                principalColumn: "PatientAppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocuments_PatientAppointment_PatientAppointmentId",
                table: "UserDocuments");

            migrationBuilder.DropIndex(
                name: "IX_UserDocuments_PatientAppointmentId",
                table: "UserDocuments");

            migrationBuilder.DropColumn(
                name: "PatientAppointmentId",
                table: "UserDocuments");
        }
    }
}
