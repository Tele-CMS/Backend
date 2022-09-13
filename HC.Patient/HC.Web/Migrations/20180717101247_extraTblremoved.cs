using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class extraTblremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientPastIllness");

            migrationBuilder.DropTable(
                name: "PatientPreference");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientPastIllness",
                columns: table => new
                {
                    PastIllnessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DiagnosisDate = table.Column<DateTime>(nullable: false),
                    Illness = table.Column<string>(maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    PatientID = table.Column<int>(nullable: false),
                    StatusID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPastIllness", x => x.PastIllnessId);
                    table.ForeignKey(
                        name: "FK_PatientPastIllness_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPastIllness_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPastIllness_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPastIllness_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientPreference",
                columns: table => new
                {
                    PatientPreferenceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AddClientToCaseLoad = table.Column<bool>(nullable: true),
                    ByEmail = table.Column<bool>(nullable: true),
                    ByPhone = table.Column<bool>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    OtherCommunication = table.Column<string>(nullable: true),
                    PatientHomeCommPreferencesID = table.Column<int>(nullable: true),
                    PatientID = table.Column<int>(nullable: false),
                    PatientOfficeCommPreferencesID = table.Column<int>(nullable: true),
                    PreferredLanguageID = table.Column<int>(nullable: true),
                    ReceiveSMS = table.Column<bool>(nullable: true),
                    ServiceRequestedID = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPreference", x => x.PatientPreferenceID);
                    table.ForeignKey(
                        name: "FK_PatientPreference_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPreference_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPreference_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPreference_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientPastIllness_CreatedBy",
                table: "PatientPastIllness",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPastIllness_DeletedBy",
                table: "PatientPastIllness",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPastIllness_PatientID",
                table: "PatientPastIllness",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPastIllness_UpdatedBy",
                table: "PatientPastIllness",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreference_CreatedBy",
                table: "PatientPreference",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreference_DeletedBy",
                table: "PatientPreference",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreference_PatientID",
                table: "PatientPreference",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreference_UpdatedBy",
                table: "PatientPreference",
                column: "UpdatedBy");
        }
    }
}
