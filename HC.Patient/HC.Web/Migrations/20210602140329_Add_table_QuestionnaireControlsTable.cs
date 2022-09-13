using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Add_table_QuestionnaireControlsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionnaireControls",
                columns: table => new
                {
                    QuestionnaireControlID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ControlName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    HasOptions = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireControls", x => x.QuestionnaireControlID);
                    table.ForeignKey(
                        name: "FK_QuestionnaireControls_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireControls_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireControls_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionnaireTypeControls",
                columns: table => new
                {
                    QuestionnaireTypeControlID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    QuestionnaireControlId = table.Column<int>(nullable: false),
                    QuestionnaireTypeId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionnaireTypeControls", x => x.QuestionnaireTypeControlID);
                    table.ForeignKey(
                        name: "FK_QuestionnaireTypeControls_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireTypeControls_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuestionnaireTypeControls_QuestionnaireControls_QuestionnaireControlId",
                        column: x => x.QuestionnaireControlId,
                        principalTable: "QuestionnaireControls",
                        principalColumn: "QuestionnaireControlID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionnaireTypeControls_MasterQuestionnaireTypes_QuestionnaireTypeId",
                        column: x => x.QuestionnaireTypeId,
                        principalTable: "MasterQuestionnaireTypes",
                        principalColumn: "QuestionnaireTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionnaireTypeControls_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireControls_CreatedBy",
                table: "QuestionnaireControls",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireControls_DeletedBy",
                table: "QuestionnaireControls",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireControls_UpdatedBy",
                table: "QuestionnaireControls",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireTypeControls_CreatedBy",
                table: "QuestionnaireTypeControls",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireTypeControls_DeletedBy",
                table: "QuestionnaireTypeControls",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireTypeControls_QuestionnaireControlId",
                table: "QuestionnaireTypeControls",
                column: "QuestionnaireControlId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireTypeControls_QuestionnaireTypeId",
                table: "QuestionnaireTypeControls",
                column: "QuestionnaireTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionnaireTypeControls_UpdatedBy",
                table: "QuestionnaireTypeControls",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionnaireTypeControls");

            migrationBuilder.DropTable(
                name: "QuestionnaireControls");
        }
    }
}
