using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class modiferchnagesinencounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Modifier4",
                table: "PatientEncounterServiceCodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier3",
                table: "PatientEncounterServiceCodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier2",
                table: "PatientEncounterServiceCodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Modifier1",
                table: "PatientEncounterServiceCodes",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier1",
                table: "PatientEncounterServiceCodes",
                column: "Modifier1");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier2",
                table: "PatientEncounterServiceCodes",
                column: "Modifier2");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier3",
                table: "PatientEncounterServiceCodes",
                column: "Modifier3");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier4",
                table: "PatientEncounterServiceCodes",
                column: "Modifier4");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier1",
                table: "PatientEncounterServiceCodes",
                column: "Modifier1",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier2",
                table: "PatientEncounterServiceCodes",
                column: "Modifier2",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier3",
                table: "PatientEncounterServiceCodes",
                column: "Modifier3",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier4",
                table: "PatientEncounterServiceCodes",
                column: "Modifier4",
                principalTable: "PayerServiceCodeModifiers",
                principalColumn: "PayerModifierId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier1",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier2",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier3",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientEncounterServiceCodes_PayerServiceCodeModifiers_Modifier4",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier1",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier2",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier3",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.DropIndex(
                name: "IX_PatientEncounterServiceCodes_Modifier4",
                table: "PatientEncounterServiceCodes");

            migrationBuilder.AlterColumn<string>(
                name: "Modifier4",
                table: "PatientEncounterServiceCodes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier3",
                table: "PatientEncounterServiceCodes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier2",
                table: "PatientEncounterServiceCodes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Modifier1",
                table: "PatientEncounterServiceCodes",
                type: "varchar(5)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
