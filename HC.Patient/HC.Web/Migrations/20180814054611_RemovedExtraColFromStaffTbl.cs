using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class RemovedExtraColFromStaffTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_MasterDiscipline_DisciplineID",
                table: "Staffs");

            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_GlobalCode_Referral",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_DisciplineID",
                table: "Staffs");

            migrationBuilder.DropIndex(
                name: "IX_Staffs_Referral",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "DisciplineID",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Referral",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Staffs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisciplineID",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Referral",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Staffs",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_DisciplineID",
                table: "Staffs",
                column: "DisciplineID");

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_Referral",
                table: "Staffs",
                column: "Referral");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_MasterDiscipline_DisciplineID",
                table: "Staffs",
                column: "DisciplineID",
                principalTable: "MasterDiscipline",
                principalColumn: "DisciplineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_GlobalCode_Referral",
                table: "Staffs",
                column: "Referral",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
