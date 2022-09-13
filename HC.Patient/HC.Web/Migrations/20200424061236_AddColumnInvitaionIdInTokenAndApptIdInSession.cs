using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddColumnInvitaionIdInTokenAndApptIdInSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitationId",
                table: "TelehealthTokenDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "TelehealthTokenDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "TelehealthSessionDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "TelehealthSessionDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthTokenDetails_InvitationId",
                table: "TelehealthTokenDetails",
                column: "InvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthTokenDetails_OrganizationId",
                table: "TelehealthTokenDetails",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthSessionDetails_OrganizationId",
                table: "TelehealthSessionDetails",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthSessionDetails_Organization_OrganizationId",
                table: "TelehealthSessionDetails",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthTokenDetails_GroupSessionInvitations_InvitationId",
                table: "TelehealthTokenDetails",
                column: "InvitationId",
                principalTable: "GroupSessionInvitations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TelehealthTokenDetails_Organization_OrganizationId",
                table: "TelehealthTokenDetails",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthSessionDetails_Organization_OrganizationId",
                table: "TelehealthSessionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthTokenDetails_GroupSessionInvitations_InvitationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TelehealthTokenDetails_Organization_OrganizationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropIndex(
                name: "IX_TelehealthTokenDetails_InvitationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropIndex(
                name: "IX_TelehealthTokenDetails_OrganizationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropIndex(
                name: "IX_TelehealthSessionDetails_OrganizationId",
                table: "TelehealthSessionDetails");

            migrationBuilder.DropColumn(
                name: "InvitationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "TelehealthTokenDetails");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "TelehealthSessionDetails");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "TelehealthSessionDetails");
        }
    }
}
