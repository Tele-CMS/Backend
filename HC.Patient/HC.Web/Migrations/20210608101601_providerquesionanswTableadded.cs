using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class providerquesionanswTableadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProviderQuestionnaireControls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProviderQuestionnaireControls",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProviderQuestionnaireAsnwers",
                columns: table => new
                {
                    ProviderQuestionnaireAsnwerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Answer = table.Column<string>(nullable: true),
                    PatientAppointmentId = table.Column<int>(nullable: false),
                    ProviderQuestionnaireControlId = table.Column<int>(nullable: false),
                    ProviderQuestionnaireControlsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderQuestionnaireAsnwers", x => x.ProviderQuestionnaireAsnwerId);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireAsnwers_PatientAppointment_PatientAppointmentId",
                        column: x => x.PatientAppointmentId,
                        principalTable: "PatientAppointment",
                        principalColumn: "PatientAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProviderQuestionnaireAsnwers_ProviderQuestionnaireControls_ProviderQuestionnaireControlsId",
                        column: x => x.ProviderQuestionnaireControlsId,
                        principalTable: "ProviderQuestionnaireControls",
                        principalColumn: "ProviderQuestionnaireControlId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireAsnwers_PatientAppointmentId",
                table: "ProviderQuestionnaireAsnwers",
                column: "PatientAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderQuestionnaireAsnwers_ProviderQuestionnaireControlsId",
                table: "ProviderQuestionnaireAsnwers",
                column: "ProviderQuestionnaireControlsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderQuestionnaireAsnwers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProviderQuestionnaireControls");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProviderQuestionnaireControls");
        }
    }
}
