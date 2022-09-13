using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddPaymentModeIntoOrganizationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "BookingCommision",
                table: "Organization",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "AppointmentPayments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_OrganizationId",
                table: "AppointmentPayments",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentPayments_Organization_OrganizationId",
                table: "AppointmentPayments",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentPayments_Organization_OrganizationId",
                table: "AppointmentPayments");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentPayments_OrganizationId",
                table: "AppointmentPayments");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "AppointmentPayments");

            migrationBuilder.AlterColumn<decimal>(
                name: "BookingCommision",
                table: "Organization",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
