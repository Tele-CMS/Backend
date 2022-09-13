using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class removedEncountercols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicianSign",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "ClinicianSignDate",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "GuardianSign",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "GuardianSignDate",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "PatientSign",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "PatientSignDate",
                table: "PatientEncounter");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ClinicianSign",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClinicianSignDate",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "GuardianSign",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GuardianSignDate",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PatientSign",
                table: "PatientEncounter",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PatientSignDate",
                table: "PatientEncounter",
                nullable: true);
        }
    }
}
