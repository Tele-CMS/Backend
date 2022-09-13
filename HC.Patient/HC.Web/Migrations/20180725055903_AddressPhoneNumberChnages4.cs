using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddressPhoneNumberChnages4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PhoneNumber",
                table: "PhoneNumbers",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Address1",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Address2",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "City",
                table: "PatientAddress",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Zip",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true);
        }
    }
}
