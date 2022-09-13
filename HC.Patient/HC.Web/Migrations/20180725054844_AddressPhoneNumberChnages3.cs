using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddressPhoneNumberChnages3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PhoneNumber",
                table: "PhoneNumbers",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Zip",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "City",
                table: "PatientAddress",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Address2",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "Address1",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "PhoneNumbers",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InsuranceIDNumber",
                table: "PatientInsuranceDetails",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Zip",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "PatientAddress",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApartmentNumber",
                table: "PatientAddress",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address2",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address1",
                table: "PatientAddress",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 1000,
                oldNullable: true);
        }
    }
}
