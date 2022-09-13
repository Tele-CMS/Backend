using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updatestaff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_MasterPayrollGroup_PayrollGroupID",
                table: "Staffs");

            migrationBuilder.AddColumn<decimal>(
                name: "PayRate",
                table: "Staffs",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_PayrollGroup_PayrollGroupID",
                table: "Staffs",
                column: "PayrollGroupID",
                principalTable: "PayrollGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_PayrollGroup_PayrollGroupID",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "PayRate",
                table: "Staffs");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_MasterPayrollGroup_PayrollGroupID",
                table: "Staffs",
                column: "PayrollGroupID",
                principalTable: "MasterPayrollGroup",
                principalColumn: "PayrollGroupID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
