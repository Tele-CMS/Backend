using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddressPhoneNumberChnages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberBCK",
                table: "PhoneNumbers",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberBCK",
                table: "PhoneNumbers");

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
    }
}
