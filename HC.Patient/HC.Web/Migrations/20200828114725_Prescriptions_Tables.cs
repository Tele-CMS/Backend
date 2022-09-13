using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Prescriptions_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientPrescriptionFaxLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityID = table.Column<int>(nullable: false),
                    CountryID = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    FaxStatus = table.Column<int>(nullable: false),
                    IsFax = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    PharmacyAddress = table.Column<string>(nullable: true),
                    PharmacyFaxNumber = table.Column<string>(nullable: true),
                    PharmacyID = table.Column<int>(nullable: false),
                    PrescriptionId = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),
                    SourceFaxNumber = table.Column<string>(nullable: true),
                    StateID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPrescriptionFaxLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionDrugs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Dose = table.Column<string>(nullable: true),
                    DrugName = table.Column<string>(nullable: true),
                    Strength = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDrugs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientPrescription",
                columns: table => new
                {
                    PatientPrescriptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Directions = table.Column<string>(nullable: true),
                    Dose = table.Column<string>(nullable: true),
                    DrugID = table.Column<int>(nullable: false),
                    Duration = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FrequencyID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    PatientID = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Strength = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPrescription", x => x.PatientPrescriptionId);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_PrescriptionDrugs_DrugID",
                        column: x => x.DrugID,
                        principalTable: "PrescriptionDrugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_GlobalCode_FrequencyID",
                        column: x => x.FrequencyID,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientPrescription_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_CreatedBy",
                table: "PatientPrescription",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_DeletedBy",
                table: "PatientPrescription",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_DrugID",
                table: "PatientPrescription",
                column: "DrugID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_FrequencyID",
                table: "PatientPrescription",
                column: "FrequencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_PatientID",
                table: "PatientPrescription",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPrescription_UpdatedBy",
                table: "PatientPrescription",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientPrescription");

            migrationBuilder.DropTable(
                name: "PatientPrescriptionFaxLog");

            migrationBuilder.DropTable(
                name: "PrescriptionDrugs");
        }
    }
}
