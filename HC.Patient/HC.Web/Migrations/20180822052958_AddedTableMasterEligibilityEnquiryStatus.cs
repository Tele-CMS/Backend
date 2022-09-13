using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedTableMasterEligibilityEnquiryStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "EDI999AcknowledgementMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MasterEligibilityEnquiryStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StatusName = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEligibilityEnquiryStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterEligibilityEnquiryStatus_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterEligibilityEnquiryStatus_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterEligibilityEnquiryStatus_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_OrganizationId",
                table: "EDI999AcknowledgementMaster",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEligibilityEnquiryStatus_CreatedBy",
                table: "MasterEligibilityEnquiryStatus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEligibilityEnquiryStatus_DeletedBy",
                table: "MasterEligibilityEnquiryStatus",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterEligibilityEnquiryStatus_UpdatedBy",
                table: "MasterEligibilityEnquiryStatus",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_EDI999AcknowledgementMaster_Organization_OrganizationId",
                table: "EDI999AcknowledgementMaster",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EDI999AcknowledgementMaster_Organization_OrganizationId",
                table: "EDI999AcknowledgementMaster");

            migrationBuilder.DropTable(
                name: "MasterEligibilityEnquiryStatus");

            migrationBuilder.DropIndex(
                name: "IX_EDI999AcknowledgementMaster_OrganizationId",
                table: "EDI999AcknowledgementMaster");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "EDI999AcknowledgementMaster");
        }
    }
}
