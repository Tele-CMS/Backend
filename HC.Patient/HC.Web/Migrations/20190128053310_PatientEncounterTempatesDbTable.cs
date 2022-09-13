using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PatientEncounterTempatesDbTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientEncounterTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    MasterTemplateId = table.Column<int>(nullable: true),
                    OrganizationID = table.Column<int>(nullable: false),
                    PatientEncounterId = table.Column<int>(nullable: true),
                    TemplateData = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientEncounterTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_MasterTemplates_MasterTemplateId",
                        column: x => x.MasterTemplateId,
                        principalTable: "MasterTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_PatientEncounter_PatientEncounterId",
                        column: x => x.PatientEncounterId,
                        principalTable: "PatientEncounter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientEncounterTemplates_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_CreatedBy",
                table: "PatientEncounterTemplates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_DeletedBy",
                table: "PatientEncounterTemplates",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_MasterTemplateId",
                table: "PatientEncounterTemplates",
                column: "MasterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_OrganizationID",
                table: "PatientEncounterTemplates",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_PatientEncounterId",
                table: "PatientEncounterTemplates",
                column: "PatientEncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientEncounterTemplates_UpdatedBy",
                table: "PatientEncounterTemplates",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientEncounterTemplates");
        }
    }
}
