using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class StartAndEndDateSchemaChangeInQualificationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "StaffQualifications");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "StaffQualifications");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StaffQualifications",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "StaffQualifications",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StaffQualifications");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "StaffQualifications");

            migrationBuilder.AddColumn<string>(
                name: "EndYear",
                table: "StaffQualifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartYear",
                table: "StaffQualifications",
                nullable: true);
        }
    }
}
