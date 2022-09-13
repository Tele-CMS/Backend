using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class EligibilityEnquiry2701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiryServiceTypeMaster_OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_EligibilityEnquiryServiceTypeMaster_Organization_OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EligibilityEnquiryServiceTypeMaster_Organization_OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster");

            migrationBuilder.DropIndex(
                name: "IX_EligibilityEnquiryServiceTypeMaster_OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "EligibilityEnquiryServiceTypeMaster");
        }
    }
}
