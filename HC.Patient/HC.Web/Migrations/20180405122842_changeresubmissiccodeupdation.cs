using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class changeresubmissiccodeupdation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim837Claims_GlobalCode_SubmissionType",
                table: "Claim837Claims");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionType",
                table: "Claim837Claims",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Claim837Claims_GlobalCode_SubmissionType",
                table: "Claim837Claims",
                column: "SubmissionType",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim837Claims_GlobalCode_SubmissionType",
                table: "Claim837Claims");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionType",
                table: "Claim837Claims",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Claim837Claims_GlobalCode_SubmissionType",
                table: "Claim837Claims",
                column: "SubmissionType",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
