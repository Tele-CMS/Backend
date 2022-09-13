using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddressPhoneNumberChnages5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "PhoneNumber",
                table: "PhoneNumbers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Address1",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Address2",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "City",
                table: "PatientAddress",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Zip",
                table: "PatientAddress",
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
