using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedFieldInEligibilityEnquiry270Master : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "EligibilityEnquiry270Master",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_StatusId",
                table: "EligibilityEnquiry270Master",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_EligibilityEnquiry270Master_MasterEligibilityEnquiryStatus_StatusId",
                table: "EligibilityEnquiry270Master",
                column: "StatusId",
                principalTable: "MasterEligibilityEnquiryStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EligibilityEnquiry270Master_MasterEligibilityEnquiryStatus_StatusId",
                table: "EligibilityEnquiry270Master");

            migrationBuilder.DropIndex(
                name: "IX_EligibilityEnquiry270Master_StatusId",
                table: "EligibilityEnquiry270Master");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "EligibilityEnquiry270Master");
        }
    }
}
