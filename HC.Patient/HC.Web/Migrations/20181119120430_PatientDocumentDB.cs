using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PatientDocumentDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DFA_PatientDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignedBy = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DFA_PatientDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_Staffs_AssignedBy",
                        column: x => x.AssignedBy,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_DFA_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DFA_Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_GlobalCode_Status",
                        column: x => x.Status,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_PatientDocuments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_AssignedBy",
                table: "DFA_PatientDocuments",
                column: "AssignedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_CreatedBy",
                table: "DFA_PatientDocuments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_DeletedBy",
                table: "DFA_PatientDocuments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_DocumentId",
                table: "DFA_PatientDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_OrganizationID",
                table: "DFA_PatientDocuments",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_PatientId",
                table: "DFA_PatientDocuments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_Status",
                table: "DFA_PatientDocuments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_PatientDocuments_UpdatedBy",
                table: "DFA_PatientDocuments",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DFA_PatientDocuments");
        }
    }
}
