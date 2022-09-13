using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class RemoveCreatedByNotNUllFromPatientTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_User_CreatedBy",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Patients",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_User_CreatedBy",
                table: "Patients",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_User_CreatedBy",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Patients",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_User_CreatedBy",
                table: "Patients",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
