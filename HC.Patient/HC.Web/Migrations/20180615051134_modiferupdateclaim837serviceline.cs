using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class modiferupdateclaim837serviceline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkWeek",
                table: "PayrollGroup",
                newName: "WorkWeekId");

            migrationBuilder.RenameColumn(
                name: "PayPeriod",
                table: "PayrollGroup",
                newName: "PayPeriodId");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "PayrollGroup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier4",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier3",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier2",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier1",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_OrganizationId",
                table: "PayrollGroup",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_PayPeriodId",
                table: "PayrollGroup",
                column: "PayPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollGroup_WorkWeekId",
                table: "PayrollGroup",
                column: "WorkWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollGroup_Organization_OrganizationId",
                table: "PayrollGroup",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollGroup_GlobalCode_PayPeriodId",
                table: "PayrollGroup",
                column: "PayPeriodId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollGroup_GlobalCode_WorkWeekId",
                table: "PayrollGroup",
                column: "WorkWeekId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollGroup_Organization_OrganizationId",
                table: "PayrollGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollGroup_GlobalCode_PayPeriodId",
                table: "PayrollGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollGroup_GlobalCode_WorkWeekId",
                table: "PayrollGroup");

            migrationBuilder.DropIndex(
                name: "IX_PayrollGroup_OrganizationId",
                table: "PayrollGroup");

            migrationBuilder.DropIndex(
                name: "IX_PayrollGroup_PayPeriodId",
                table: "PayrollGroup");

            migrationBuilder.DropIndex(
                name: "IX_PayrollGroup_WorkWeekId",
                table: "PayrollGroup");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "PayrollGroup");

            migrationBuilder.RenameColumn(
                name: "WorkWeekId",
                table: "PayrollGroup",
                newName: "WorkWeek");

            migrationBuilder.RenameColumn(
                name: "PayPeriodId",
                table: "PayrollGroup",
                newName: "PayPeriod");

            migrationBuilder.AlterColumn<int>(
                name: "Modifier4",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier3",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier2",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier1",
                table: "Claim837ServiceLine",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
