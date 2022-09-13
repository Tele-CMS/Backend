using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updateutat1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_LocationId",
                table: "UserTimesheetByAppointmentType",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_Location_LocationId",
                table: "UserTimesheetByAppointmentType",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_Location_LocationId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheetByAppointmentType_LocationId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "UserTimesheetByAppointmentType");
        }
    }
}
