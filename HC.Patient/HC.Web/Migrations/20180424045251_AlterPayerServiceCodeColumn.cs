using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AlterPayerServiceCodeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RoundingRuleID",
                table: "PayerServiceCodes");

            migrationBuilder.RenameColumn(
                name: "RoundingRuleID",
                table: "PayerServiceCodes",
                newName: "RuleID");

            migrationBuilder.RenameIndex(
                name: "IX_PayerServiceCodes_RoundingRuleID",
                table: "PayerServiceCodes",
                newName: "IX_PayerServiceCodes_RuleID");

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes",
                column: "RuleID",
                principalTable: "MasterRoundingRules",
                principalColumn: "RuleID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes");

            migrationBuilder.RenameColumn(
                name: "RuleID",
                table: "PayerServiceCodes",
                newName: "RoundingRuleID");

            migrationBuilder.RenameIndex(
                name: "IX_PayerServiceCodes_RuleID",
                table: "PayerServiceCodes",
                newName: "IX_PayerServiceCodes_RoundingRuleID");

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RoundingRuleID",
                table: "PayerServiceCodes",
                column: "RoundingRuleID",
                principalTable: "MasterRoundingRules",
                principalColumn: "RuleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
