using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatedPRimaryProviderColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Staffs_PrimaryProvider",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PrimaryProvider",
                table: "Patients");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryProvider",
                table: "Patients",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "Patients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_StaffId",
                table: "Patients",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Staffs_StaffId",
                table: "Patients",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Staffs_StaffId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_StaffId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryProvider",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PrimaryProvider",
                table: "Patients",
                column: "PrimaryProvider");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Staffs_PrimaryProvider",
                table: "Patients",
                column: "PrimaryProvider",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
