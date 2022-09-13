using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Add_table_QuestionnaireTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterQuestionnaireTypes",
                columns: table => new
                {
                    QuestionnaireTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    QuestionnaireType = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterQuestionnaireTypes", x => x.QuestionnaireTypeID);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireTypes_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireTypes_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireTypes_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterQuestionnaireTypes_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireTypes_CreatedBy",
                table: "MasterQuestionnaireTypes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireTypes_DeletedBy",
                table: "MasterQuestionnaireTypes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireTypes_OrganizationID",
                table: "MasterQuestionnaireTypes",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterQuestionnaireTypes_UpdatedBy",
                table: "MasterQuestionnaireTypes",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterQuestionnaireTypes");
        }
    }
}
