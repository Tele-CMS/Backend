using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MasterDocumentTypesStaffId",
                table: "UserDocuments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherDocumentType",
                table: "UserDocuments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MasterDocumentTypesStaff",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDocumentTypesStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterDocumentTypesStaff_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterDocumentTypesStaff_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterDocumentTypesStaff_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterDocumentTypesStaff_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDocuments_MasterDocumentTypesStaffId",
                table: "UserDocuments",
                column: "MasterDocumentTypesStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDocumentTypesStaff_CreatedBy",
                table: "MasterDocumentTypesStaff",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDocumentTypesStaff_DeletedBy",
                table: "MasterDocumentTypesStaff",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDocumentTypesStaff_OrganizationID",
                table: "MasterDocumentTypesStaff",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDocumentTypesStaff_UpdatedBy",
                table: "MasterDocumentTypesStaff",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_MasterDocumentTypesStaffId",
                table: "UserDocuments",
                column: "MasterDocumentTypesStaffId",
                principalTable: "MasterDocumentTypesStaff",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDocuments_MasterDocumentTypesStaff_MasterDocumentTypesStaffId",
                table: "UserDocuments");

            migrationBuilder.DropTable(
                name: "MasterDocumentTypesStaff");

            migrationBuilder.DropIndex(
                name: "IX_UserDocuments_MasterDocumentTypesStaffId",
                table: "UserDocuments");

            migrationBuilder.DropColumn(
                name: "MasterDocumentTypesStaffId",
                table: "UserDocuments");

            migrationBuilder.DropColumn(
                name: "OtherDocumentType",
                table: "UserDocuments");
        }
    }
}
