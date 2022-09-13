using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewTableProviderQuestionnaires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionnareId",
                table: "ProviderQuestionnaireQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProviderQuestionnaires",
                columns: table => new
                {
                    QuestionnareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    QuestionnnaireName = table.Column<string>(type: "varchar(1500)", nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderQuestionnaires", x => x.QuestionnareId);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaires_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaires_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaires_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaires_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireQuestions_QuestionnareId",
                table: "ProviderQuestionnaireQuestions",
                column: "QuestionnareId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaires_CreatedBy",
                table: "ProviderQuestionnaires",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaires_DeletedBy",
                table: "ProviderQuestionnaires",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaires_OrganizationId",
                table: "ProviderQuestionnaires",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaires_UpdatedBy",
                table: "ProviderQuestionnaires",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderQuestionnaireQuestions_ProviderQuestionnaires_QuestionnareId",
                table: "ProviderQuestionnaireQuestions",
                column: "QuestionnareId",
                principalTable: "ProviderQuestionnaires",
                principalColumn: "QuestionnareId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderQuestionnaireQuestions_ProviderQuestionnaires_QuestionnareId",
                table: "ProviderQuestionnaireQuestions");

            migrationBuilder.DropTable(
                name: "ProviderQuestionnaires");

            migrationBuilder.DropIndex(
                name: "IX_ProviderQuestionnaireQuestions_QuestionnareId",
                table: "ProviderQuestionnaireQuestions");

            migrationBuilder.DropColumn(
                name: "QuestionnareId",
                table: "ProviderQuestionnaireQuestions");
        }
    }
}
