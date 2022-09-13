using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddColumnModeAndTypePatientAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.AddColumn<string>(
                name: "BookingMode",
                table: "PatientAppointment",
                type: "varchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingType",
                table: "PatientAppointment",
                type: "varchar(50)",
                nullable: true);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.DropColumn(
                name: "BookingMode",
                table: "PatientAppointment");

            migrationBuilder.DropColumn(
                name: "BookingType",
                table: "PatientAppointment");

           
        }
    }
}
