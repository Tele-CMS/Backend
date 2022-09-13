using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedFieldRelatedToTimeZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Offset",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "Location");

            migrationBuilder.AddColumn<decimal>(
                name: "RatePerUnit",
                table: "PayerAppointmentTypes",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DaylightSavingTime",
                table: "MasterState",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StandardTime",
                table: "MasterState",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DaylightSavingTime",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StandardTime",
                table: "Location",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatePerUnit",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropColumn(
                name: "DaylightSavingTime",
                table: "MasterState");

            migrationBuilder.DropColumn(
                name: "StandardTime",
                table: "MasterState");

            migrationBuilder.DropColumn(
                name: "DaylightSavingTime",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "StandardTime",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "Offset",
                table: "Location",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "Location",
                nullable: true);
        }
    }
}
