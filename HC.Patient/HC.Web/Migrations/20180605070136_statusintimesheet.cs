using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class statusintimesheet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserTimesheet",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserDetailedDriveTime",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_Status",
                table: "UserTimesheet",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_Status",
                table: "UserDetailedDriveTime",
                column: "Status");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetailedDriveTime_GlobalCode_Status",
                table: "UserDetailedDriveTime");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheet_GlobalCode_Status",
                table: "UserTimesheet");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheet_Status",
                table: "UserTimesheet");

            migrationBuilder.DropIndex(
                name: "IX_UserDetailedDriveTime_Status",
                table: "UserDetailedDriveTime");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserTimesheet");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserDetailedDriveTime");
        }
    }
}
