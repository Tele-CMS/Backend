using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PayrollChangesss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDuration",
                table: "UserTimesheetByAppointmentType",
                newName: "ExpectedTimeDuration");

            migrationBuilder.AddColumn<decimal>(
                name: "ActualTimeDuration",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PayrollBreakTimeId",
                table: "PayrollGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_PayrollBreakTimeId",
                table: "PayrollGroup",
                column: "PayrollBreakTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollGroup_PayrollBreakTime_PayrollBreakTimeId",
                table: "PayrollGroup",
                column: "PayrollBreakTimeId",
                principalTable: "PayrollBreakTime",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollGroup_PayrollBreakTime_PayrollBreakTimeId",
                table: "PayrollGroup");

            migrationBuilder.DropIndex(
                name: "IX_PayrollGroup_PayrollBreakTimeId",
                table: "PayrollGroup");

            migrationBuilder.DropColumn(
                name: "ActualTimeDuration",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "PayrollBreakTimeId",
                table: "PayrollGroup");

            migrationBuilder.RenameColumn(
                name: "ExpectedTimeDuration",
                table: "UserTimesheetByAppointmentType",
                newName: "TotalDuration");
        }
    }
}
