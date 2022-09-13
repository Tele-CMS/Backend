using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updatestatusintimesheet3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDurationForDay",
                table: "UserTimesheetByAppointmentType",
                newName: "TotalDuration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDuration",
                table: "UserTimesheetByAppointmentType",
                newName: "TotalDurationForDay");
        }
    }
}
