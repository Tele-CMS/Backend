using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AlterPayerServiceCodeColumnNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes");

            migrationBuilder.AlterColumn<int>(
                name: "RuleID",
                table: "PayerServiceCodes",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes",
                column: "RuleID",
                principalTable: "MasterRoundingRules",
                principalColumn: "RuleID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes");

            migrationBuilder.AlterColumn<int>(
                name: "RuleID",
                table: "PayerServiceCodes",
                nullable: true,
                oldClrType: typeof(int),
                oldDefaultValue: 1);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodes_MasterRoundingRules_RuleID",
                table: "PayerServiceCodes",
                column: "RuleID",
                principalTable: "MasterRoundingRules",
                principalColumn: "RuleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
