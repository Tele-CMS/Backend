using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatePHIDatInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SSN",
                table: "Patients",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "SSN",
                table: "Patients",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
