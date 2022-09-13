using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Add_table_ProvidersQuestionnaireControlsTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProviderQuestionnaireControls",
                columns: table => new
                {
                    ProviderQuestionnaireControlId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Options = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    QuestionText = table.Column<string>(nullable: true),
                    QuestionnaireTypeControlId = table.Column<int>(nullable: false),
                    StaffID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderQuestionnaireControls", x => x.ProviderQuestionnaireControlId);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireControls_QuestionnaireTypeControls_QuestionnaireTypeControlId",
                        column: x => x.QuestionnaireTypeControlId,
                        principalTable: "QuestionnaireTypeControls",
                        principalColumn: "QuestionnaireTypeControlID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireControls_QuestionnaireTypeControlId",
                table: "ProviderQuestionnaireControls",
                column: "QuestionnaireTypeControlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderQuestionnaireControls");
        }
    }
}
