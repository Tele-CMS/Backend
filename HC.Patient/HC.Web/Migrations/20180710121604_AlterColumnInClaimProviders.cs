using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AlterColumnInClaimProviders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimProviders_Claims_ClaimId",
                table: "ClaimProviders");

            migrationBuilder.RenameColumn(
                name: "ClaimId",
                table: "ClaimProviders",
                newName: "ClaimServiceLineId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimProviders_ClaimId",
                table: "ClaimProviders",
                newName: "IX_ClaimProviders_ClaimServiceLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimProviders_ClaimServiceLine_ClaimServiceLineId",
                table: "ClaimProviders",
                column: "ClaimServiceLineId",
                principalTable: "ClaimServiceLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimProviders_ClaimServiceLine_ClaimServiceLineId",
                table: "ClaimProviders");

            migrationBuilder.RenameColumn(
                name: "ClaimServiceLineId",
                table: "ClaimProviders",
                newName: "ClaimId");

            migrationBuilder.RenameIndex(
                name: "IX_ClaimProviders_ClaimServiceLineId",
                table: "ClaimProviders",
                newName: "IX_ClaimProviders_ClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimProviders_Claims_ClaimId",
                table: "ClaimProviders",
                column: "ClaimId",
                principalTable: "Claims",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
