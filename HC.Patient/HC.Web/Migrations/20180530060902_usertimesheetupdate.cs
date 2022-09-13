using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class usertimesheetupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentTypeId",
                table: "UserTimesheet");

            migrationBuilder.DropColumn(
                name: "DateRendered",
                table: "UserTimesheet");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "UserTimesheet",
                newName: "ServiceDuration");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceDuration",
                table: "UserTimesheet",
                newName: "Duration");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentTypeId",
                table: "UserTimesheet",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateRendered",
                table: "UserTimesheet",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
