using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddTableTelehealthRecordingDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TelehealthRecordingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ArchiveId = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: true),
                    TelehealthSessionDetailID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelehealthRecordingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelehealthRecordingDetails_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TelehealthRecordingDetails_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TelehealthRecordingDetails_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TelehealthRecordingDetails_TelehealthSessionDetails_TelehealthSessionDetailID",
                        column: x => x.TelehealthSessionDetailID,
                        principalTable: "TelehealthSessionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TelehealthRecordingDetails_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthRecordingDetails_CreatedBy",
                table: "TelehealthRecordingDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthRecordingDetails_DeletedBy",
                table: "TelehealthRecordingDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthRecordingDetails_OrganizationId",
                table: "TelehealthRecordingDetails",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthRecordingDetails_TelehealthSessionDetailID",
                table: "TelehealthRecordingDetails",
                column: "TelehealthSessionDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_TelehealthRecordingDetails_UpdatedBy",
                table: "TelehealthRecordingDetails",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TelehealthRecordingDetails");
        }
    }
}
