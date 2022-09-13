using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PayerServiceAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Modifier4",
                table: "PayerAppointmentTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier3",
                table: "PayerAppointmentTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier2",
                table: "PayerAppointmentTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier1",
                table: "PayerAppointmentTypes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Modifier4",
                table: "PayerAppointmentTypes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier3",
                table: "PayerAppointmentTypes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier2",
                table: "PayerAppointmentTypes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier1",
                table: "PayerAppointmentTypes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
