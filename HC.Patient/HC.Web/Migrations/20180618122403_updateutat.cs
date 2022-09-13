using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updateutat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_OrganizationId",
                table: "UserTimesheetByAppointmentType",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_Organization_OrganizationId",
                table: "UserTimesheetByAppointmentType",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_Organization_OrganizationId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheetByAppointmentType_OrganizationId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "UserTimesheetByAppointmentType");
        }
    }
}
