using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewTableProviderQuestionnaireQuestionsAndQuestionnaireOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderQuestionnaireQuestions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    QuestionNameName = table.Column<string>(type: "varchar(1500)", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderQuestionnaireQuestions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireQuestions_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireQuestions_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireQuestions_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireQuestions_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionnaireOptions",
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
                    QuestionId = table.Column<int>(nullable: false),
                    QuestionOptionName = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionnaireOptions_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireOptions_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireOptions_ProviderQuestionnaireQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "ProviderQuestionnaireQuestions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionnaireOptions_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireQuestions_CreatedBy",
                table: "ProviderQuestionnaireQuestions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireQuestions_DeletedBy",
                table: "ProviderQuestionnaireQuestions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireQuestions_OrganizationId",
                table: "ProviderQuestionnaireQuestions",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireQuestions_UpdatedBy",
                table: "ProviderQuestionnaireQuestions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireOptions_CreatedBy",
                table: "QuestionnaireOptions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireOptions_DeletedBy",
                table: "QuestionnaireOptions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireOptions_QuestionId",
                table: "QuestionnaireOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireOptions_UpdatedBy",
                table: "QuestionnaireOptions",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionnaireOptions");

            migrationBuilder.DropTable(
                name: "ProviderQuestionnaireQuestions");
        }
    }
}
