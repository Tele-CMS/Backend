using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnInClaim : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimPaymentStatusId",
                table: "Claims",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimPaymentStatusId",
                table: "Claims",
                column: "ClaimPaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_GlobalCode_ClaimPaymentStatusId",
                table: "Claims",
                column: "ClaimPaymentStatusId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_GlobalCode_ClaimPaymentStatusId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ClaimPaymentStatusId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ClaimPaymentStatusId",
                table: "Claims");
        }
    }
}
