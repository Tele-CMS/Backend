using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedFieldinStaffLeave : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveStatus",
                table: "StaffLeave");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovalDate",
                table: "StaffLeave",
                nullable: false,
                defaultValueSql: "GetUtcDate()");

            migrationBuilder.AddColumn<string>(
                name: "DeclineReason",
                table: "StaffLeave",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LeaveStatusId",
                table: "StaffLeave",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StaffLeave_LeaveStatusId",
                table: "StaffLeave",
                column: "LeaveStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffLeave_GlobalCode_LeaveStatusId",
                table: "StaffLeave",
                column: "LeaveStatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffLeave_GlobalCode_LeaveStatusId",
                table: "StaffLeave");

            migrationBuilder.DropIndex(
                name: "IX_StaffLeave_LeaveStatusId",
                table: "StaffLeave");

            migrationBuilder.DropColumn(
                name: "ApprovalDate",
                table: "StaffLeave");

            migrationBuilder.DropColumn(
                name: "DeclineReason",
                table: "StaffLeave");

            migrationBuilder.DropColumn(
                name: "LeaveStatusId",
                table: "StaffLeave");

            migrationBuilder.AddColumn<int>(
                name: "LeaveStatus",
                table: "StaffLeave",
                nullable: true);
        }
    }
}
