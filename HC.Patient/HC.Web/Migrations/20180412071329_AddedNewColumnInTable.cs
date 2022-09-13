using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnInTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_MasterDocumentTypesStaffId",
                table: "UserDocuments");

            migrationBuilder.RenameColumn(
                name: "MasterDocumentTypesStaffId",
                table: "UserDocuments",
                newName: "DocumentTypeIdStaff");

            migrationBuilder.RenameIndex(
                name: "IX_UserDocuments_MasterDocumentTypesStaffId",
                table: "UserDocuments",
                newName: "IX_UserDocuments_DocumentTypeIdStaff");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_DocumentTypeIdStaff",
                table: "UserDocuments",
                column: "DocumentTypeIdStaff",
                principalTable: "MasterDocumentTypesStaff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_DocumentTypeIdStaff",
                table: "UserDocuments");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeIdStaff",
                table: "UserDocuments",
                newName: "MasterDocumentTypesStaffId");

            migrationBuilder.RenameIndex(
                name: "IX_UserDocuments_DocumentTypeIdStaff",
                table: "UserDocuments",
                newName: "IX_UserDocuments_MasterDocumentTypesStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_MasterDocumentTypesStaffId",
                table: "UserDocuments",
                column: "MasterDocumentTypesStaffId",
                principalTable: "MasterDocumentTypesStaff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
