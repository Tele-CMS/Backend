using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class DocumentAnswerdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DFA_DocumentAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DocumentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    PatientID = table.Column<int>(nullable: false),
                    SectionItemId = table.Column<int>(nullable: false),
                    TextAnswer = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DFA_DocumentAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_DFA_CategoryCode_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "DFA_CategoryCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_DFA_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "DFA_Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_DFA_SectionItem_SectionItemId",
                        column: x => x.SectionItemId,
                        principalTable: "DFA_SectionItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DFA_DocumentAnswer_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_AnswerId",
                table: "DFA_DocumentAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_CreatedBy",
                table: "DFA_DocumentAnswer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_DeletedBy",
                table: "DFA_DocumentAnswer",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_DocumentId",
                table: "DFA_DocumentAnswer",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_PatientID",
                table: "DFA_DocumentAnswer",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_SectionItemId",
                table: "DFA_DocumentAnswer",
                column: "SectionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DFA_DocumentAnswer_UpdatedBy",
                table: "DFA_DocumentAnswer",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DFA_DocumentAnswer");
        }
    }
}
