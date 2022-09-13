using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PayerServiceAppointmentForgineKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PayerAppointmentTypes_Modifier1",
                table: "PayerAppointmentTypes",
                column: "Modifier1");

            migrationBuilder.CreateIndex(
                name: "IX_PayerAppointmentTypes_Modifier2",
                table: "PayerAppointmentTypes",
                column: "Modifier2");

            migrationBuilder.CreateIndex(
                name: "IX_PayerAppointmentTypes_Modifier3",
                table: "PayerAppointmentTypes",
                column: "Modifier3");

            migrationBuilder.CreateIndex(
                name: "IX_PayerAppointmentTypes_Modifier4",
                table: "PayerAppointmentTypes",
                column: "Modifier4");

            migrationBuilder.AddForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier1",
                table: "PayerAppointmentTypes",
                column: "Modifier1",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier2",
                table: "PayerAppointmentTypes",
                column: "Modifier2",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier3",
                table: "PayerAppointmentTypes",
                column: "Modifier3",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier4",
                table: "PayerAppointmentTypes",
                column: "Modifier4",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier1",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier2",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier3",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_PayerAppointmentTypes_PayerServiceCodeModifiers_Modifier4",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PayerAppointmentTypes_Modifier1",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PayerAppointmentTypes_Modifier2",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PayerAppointmentTypes_Modifier3",
                table: "PayerAppointmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_PayerAppointmentTypes_Modifier4",
                table: "PayerAppointmentTypes");
        }
    }
}
