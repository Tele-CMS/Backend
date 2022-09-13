using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class InvitedAppointmentIdInGroupSessionInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitedAppointmentId",
                table: "GroupSessionInvitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_InvitedAppointmentId",
                table: "GroupSessionInvitations",
                column: "InvitedAppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSessionInvitations_PatientAppointment_InvitedAppointmentId",
                table: "GroupSessionInvitations",
                column: "InvitedAppointmentId",
                principalTable: "PatientAppointment",
                principalColumn: "PatientAppointmentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSessionInvitations_PatientAppointment_InvitedAppointmentId",
                table: "GroupSessionInvitations");

            migrationBuilder.DropIndex(
                name: "IX_GroupSessionInvitations_InvitedAppointmentId",
                table: "GroupSessionInvitations");

            migrationBuilder.DropColumn(
                name: "InvitedAppointmentId",
                table: "GroupSessionInvitations");
        }
    }
}
