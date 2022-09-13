using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class RemovedExtraColFromPatientTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_GlobalCode_ClientStatus",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MasterProgram_Program",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_GlobalCode_Referral",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_GlobalCode_Suffix",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ClientStatus",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Program",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Referral",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Suffix",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ClientStatus",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Program",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ProgramStartDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Referral",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ReferralDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ReferralReason",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Suffix",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientID",
                table: "Patients",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientStatus",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Patients",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Patients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Program",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProgramStartDate",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Referral",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReferralDate",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferralReason",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Suffix",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Patients",
                maxLength: 10,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClientStatus",
                table: "Patients",
                column: "ClientStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Program",
                table: "Patients",
                column: "Program");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Referral",
                table: "Patients",
                column: "Referral");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Suffix",
                table: "Patients",
                column: "Suffix");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_GlobalCode_ClientStatus",
                table: "Patients",
                column: "ClientStatus",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MasterProgram_Program",
                table: "Patients",
                column: "Program",
                principalTable: "MasterProgram",
                principalColumn: "ProgramID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_GlobalCode_Referral",
                table: "Patients",
                column: "Referral",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_GlobalCode_Suffix",
                table: "Patients",
                column: "Suffix",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
