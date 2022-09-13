using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MakeAppointmentTypeNullableInPatientAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeID",
                table: "PatientAppointment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment",
                column: "AppointmentTypeID",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeID",
                table: "PatientAppointment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment",
                column: "AppointmentTypeID",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
