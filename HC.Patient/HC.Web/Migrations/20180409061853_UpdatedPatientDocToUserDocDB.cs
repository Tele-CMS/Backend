using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatedPatientDocToUserDocDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientDocuments");

            migrationBuilder.CreateTable(
                name: "UserDocuments",
                columns: table => new
                {
                    UserDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DocumentName = table.Column<string>(maxLength: 100, nullable: true),
                    DocumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentTypeId = table.Column<int>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UploadPath = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDocuments", x => x.UserDocumentId);
                    table.ForeignKey(
                        name: "FK_UserDocuments_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_MasterDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "MasterDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDocuments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_CreatedBy",
                table: "UserDocuments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_DeletedBy",
                table: "UserDocuments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_DocumentTypeId",
                table: "UserDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UpdatedBy",
                table: "UserDocuments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_UserId",
                table: "UserDocuments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDocuments");

            migrationBuilder.CreateTable(
                name: "PatientDocuments",
                columns: table => new
                {
                    PatientDocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DocumentName = table.Column<string>(maxLength: 100, nullable: true),
                    DocumentNumber = table.Column<string>(maxLength: 50, nullable: true),
                    DocumentTypeId = table.Column<int>(nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    PatientID = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UploadPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDocuments", x => x.PatientDocumentId);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_MasterDocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "MasterDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDocuments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_CreatedBy",
                table: "PatientDocuments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_DeletedBy",
                table: "PatientDocuments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_DocumentTypeId",
                table: "PatientDocuments",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_PatientID",
                table: "PatientDocuments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDocuments_UpdatedBy",
                table: "PatientDocuments",
                column: "UpdatedBy");
        }
    }
}
