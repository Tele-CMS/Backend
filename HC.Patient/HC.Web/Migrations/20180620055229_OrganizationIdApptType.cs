using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class OrganizationIdApptType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentType_Organization_OrganizationID",
                table: "AppointmentType");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationID",
                table: "AppointmentType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentType_Organization_OrganizationID",
                table: "AppointmentType",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentType_Organization_OrganizationID",
                table: "AppointmentType");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationID",
                table: "AppointmentType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentType_Organization_OrganizationID",
                table: "AppointmentType",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
