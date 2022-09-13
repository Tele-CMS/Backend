using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updatestatusintimesheet2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_StatusId",
                table: "UserTimesheetByAppointmentType",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_GlobalCode_StatusId",
                table: "UserTimesheetByAppointmentType",
                column: "StatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_GlobalCode_StatusId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheetByAppointmentType_StatusId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "UserTimesheetByAppointmentType");
        }
    }
}
