using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnsForAddresses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Staffs",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "PatientAppointment",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Organization",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "Location",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "InsuredPerson",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "InsuranceCompanies",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "InsuredPerson");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "InsuranceCompanies");
        }
    }
}
