using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class DroppedBackUpColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberBCK",
                table: "PhoneNumbers");

            migrationBuilder.DropColumn(
                name: "DOBBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "EmailBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FirstNameBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastNameBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MRNBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MiddleNameBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "SSNBCK",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceIDNumberBCK",
                table: "PatientInsuranceDetails");

            migrationBuilder.DropColumn(
                name: "Address1BCK",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "Address2BCK",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "ApartmentNumberBCK",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "CityBCK",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "ZipBCK",
                table: "PatientAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberBCK",
                table: "PhoneNumbers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DOBBCK",
                table: "Patients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EmailBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstNameBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MRNBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleNameBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SSNBCK",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceIDNumberBCK",
                table: "PatientInsuranceDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1BCK",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2BCK",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumberBCK",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityBCK",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipBCK",
                table: "PatientAddress",
                nullable: true);
        }
    }
}
