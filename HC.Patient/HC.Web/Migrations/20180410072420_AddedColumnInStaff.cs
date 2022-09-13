using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedColumnInStaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityTypeID",
                table: "AppointmentType");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeID",
                table: "Staffs",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Staffs");

            migrationBuilder.AddColumn<string>(
                name: "ActivityTypeID",
                table: "AppointmentType",
                maxLength: 100,
                nullable: true);
        }
    }
}
