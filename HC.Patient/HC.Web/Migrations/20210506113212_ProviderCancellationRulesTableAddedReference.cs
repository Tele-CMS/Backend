using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ProviderCancellationRulesTableAddedReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProviderCancellationRules_StaffId",
                table: "ProviderCancellationRules",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderCancellationRules_Staffs_StaffId",
                table: "ProviderCancellationRules",
                column: "StaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderCancellationRules_Staffs_StaffId",
                table: "ProviderCancellationRules");

            migrationBuilder.DropIndex(
                name: "IX_ProviderCancellationRules_StaffId",
                table: "ProviderCancellationRules");
        }
    }
}
