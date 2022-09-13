using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatePHIDatInfo6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DOB",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MRN",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "SSN",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DOB",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Email",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FirstName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "LastName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MRN",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "MiddleName",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SSN",
                table: "Patients",
                nullable: true);
        }
    }
}
