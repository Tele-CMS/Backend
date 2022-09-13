using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updatestatusintimesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetailedDriveTime_GlobalCode_Status",
                table: "UserDetailedDriveTime");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheet_GlobalCode_Status",
                table: "UserTimesheet");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserTimesheet",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTimesheet_Status",
                table: "UserTimesheet",
                newName: "IX_UserTimesheet_StatusId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "UserDetailedDriveTime",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetailedDriveTime_Status",
                table: "UserDetailedDriveTime",
                newName: "IX_UserDetailedDriveTime_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetailedDriveTime_GlobalCode_StatusId",
                table: "UserDetailedDriveTime",
                column: "StatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheet_GlobalCode_StatusId",
                table: "UserTimesheet",
                column: "StatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetailedDriveTime_GlobalCode_StatusId",
                table: "UserDetailedDriveTime");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheet_GlobalCode_StatusId",
                table: "UserTimesheet");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "UserTimesheet",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_UserTimesheet_StatusId",
                table: "UserTimesheet",
                newName: "IX_UserTimesheet_Status");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "UserDetailedDriveTime",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_UserDetailedDriveTime_StatusId",
                table: "UserDetailedDriveTime",
                newName: "IX_UserDetailedDriveTime_Status");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetailedDriveTime_GlobalCode_Status",
                table: "UserDetailedDriveTime",
                column: "Status",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheet_GlobalCode_Status",
                table: "UserTimesheet",
                column: "Status",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
