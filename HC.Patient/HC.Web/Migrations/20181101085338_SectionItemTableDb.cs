using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class SectionItemTableDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DFA_SectionItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ItemLabel = table.Column<string>(nullable: true),
                    Itemtype = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DFA_SectionItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_DFA_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "DFA_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_DFA_CategoryCode_Itemtype",
                        column: x => x.Itemtype,
                        principalTable: "DFA_CategoryCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_DFA_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "DFA_Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_SectionItem_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_CategoryId",
                table: "DFA_SectionItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_CreatedBy",
                table: "DFA_SectionItem",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_DeletedBy",
                table: "DFA_SectionItem",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_Itemtype",
                table: "DFA_SectionItem",
                column: "Itemtype");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_SectionId",
                table: "DFA_SectionItem",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_SectionItem_UpdatedBy",
                table: "DFA_SectionItem",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DFA_SectionItem");
        }
    }
}
