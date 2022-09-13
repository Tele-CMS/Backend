using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddressPhoneNumberChnages2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "PhoneNumbers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "PatientAddress",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "PhoneNumbers");

            migrationBuilder.DropColumn(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails");

            migrationBuilder.DropColumn(
                name: "Address1",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "ApartmentNumber",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "City",
                table: "PatientAddress");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "PatientAddress");
        }
    }
}
