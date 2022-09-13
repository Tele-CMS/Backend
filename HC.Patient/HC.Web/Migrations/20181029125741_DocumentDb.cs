using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class DocumentDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFA_Category_Organization_OrganizationID",
                table: "DFA_Category");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationID",
                table: "DFA_Category",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DFA_Document",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    DocumentName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DFA_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DFA_Document_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_Document_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_Document_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_Document_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DFA_Document_CreatedBy",
                table: "DFA_Document",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_Document_DeletedBy",
                table: "DFA_Document",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_Document_OrganizationID",
                table: "DFA_Document",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_Document_UpdatedBy",
                table: "DFA_Document",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_DFA_Category_Organization_OrganizationID",
                table: "DFA_Category",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFA_Category_Organization_OrganizationID",
                table: "DFA_Category");

            migrationBuilder.DropTable(
                name: "DFA_Document");

            migrationBuilder.AlterColumn<int>(
                name: "OrganizationID",
                table: "DFA_Category",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DFA_Category_Organization_OrganizationID",
                table: "DFA_Category",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
