using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class EligibilityEnquiry270 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EligibilityEnquiry270Master",
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
                    OrganizationId = table.Column<int>(nullable: false),
                    PatientId = table.Column<int>(nullable: false),
                    PatientInsuranceId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityEnquiry270Master", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_PatientInsuranceDetails_PatientInsuranceId",
                        column: x => x.PatientInsuranceId,
                        principalTable: "PatientInsuranceDetails",
                        principalColumn: "PatientInsuranceID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270Master_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityEnquiryServiceTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(type: "varchar(1000)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ServiceTypeCode = table.Column<string>(type: "varchar(5)", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityEnquiryServiceTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiryServiceTypeMaster_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiryServiceTypeMaster_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiryServiceTypeMaster_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityEnquiry270ServiceCodesDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EligibilityEnquiry270MasterId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ServiceCodeId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityEnquiry270ServiceCodesDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceCodesDetails_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceCodesDetails_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceCodesDetails_EligibilityEnquiry270Master_EligibilityEnquiry270MasterId",
                        column: x => x.EligibilityEnquiry270MasterId,
                        principalTable: "EligibilityEnquiry270Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceCodesDetails_MasterServiceCode_ServiceCodeId",
                        column: x => x.ServiceCodeId,
                        principalTable: "MasterServiceCode",
                        principalColumn: "ServiceCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceCodesDetails_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityEnquiry270ServiceTypeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EligibilityEnquiry270MasterId = table.Column<int>(nullable: false),
                    EligibilityEnquiryServiceTypeMasterId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityEnquiry270ServiceTypeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceTypeDetails_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceTypeDetails_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceTypeDetails_EligibilityEnquiry270Master_EligibilityEnquiry270MasterId",
                        column: x => x.EligibilityEnquiry270MasterId,
                        principalTable: "EligibilityEnquiry270Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceTypeDetails_EligibilityEnquiryServiceTypeMaster_EligibilityEnquiryServiceTypeMasterId",
                        column: x => x.EligibilityEnquiryServiceTypeMasterId,
                        principalTable: "EligibilityEnquiryServiceTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EligibilityEnquiry270ServiceTypeDetails_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_CreatedBy",
                table: "EligibilityEnquiry270Master",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_DeletedBy",
                table: "EligibilityEnquiry270Master",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_OrganizationId",
                table: "EligibilityEnquiry270Master",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_PatientId",
                table: "EligibilityEnquiry270Master",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_PatientInsuranceId",
                table: "EligibilityEnquiry270Master",
                column: "PatientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270Master_UpdatedBy",
                table: "EligibilityEnquiry270Master",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceCodesDetails_CreatedBy",
                table: "EligibilityEnquiry270ServiceCodesDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceCodesDetails_DeletedBy",
                table: "EligibilityEnquiry270ServiceCodesDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceCodesDetails_EligibilityEnquiry270MasterId",
                table: "EligibilityEnquiry270ServiceCodesDetails",
                column: "EligibilityEnquiry270MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceCodesDetails_ServiceCodeId",
                table: "EligibilityEnquiry270ServiceCodesDetails",
                column: "ServiceCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceCodesDetails_UpdatedBy",
                table: "EligibilityEnquiry270ServiceCodesDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceTypeDetails_CreatedBy",
                table: "EligibilityEnquiry270ServiceTypeDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceTypeDetails_DeletedBy",
                table: "EligibilityEnquiry270ServiceTypeDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceTypeDetails_EligibilityEnquiry270MasterId",
                table: "EligibilityEnquiry270ServiceTypeDetails",
                column: "EligibilityEnquiry270MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceTypeDetails_EligibilityEnquiryServiceTypeMasterId",
                table: "EligibilityEnquiry270ServiceTypeDetails",
                column: "EligibilityEnquiryServiceTypeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiry270ServiceTypeDetails_UpdatedBy",
                table: "EligibilityEnquiry270ServiceTypeDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiryServiceTypeMaster_CreatedBy",
                table: "EligibilityEnquiryServiceTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiryServiceTypeMaster_DeletedBy",
                table: "EligibilityEnquiryServiceTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityEnquiryServiceTypeMaster_UpdatedBy",
                table: "EligibilityEnquiryServiceTypeMaster",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EligibilityEnquiry270ServiceCodesDetails");

            migrationBuilder.DropTable(
                name: "EligibilityEnquiry270ServiceTypeDetails");

            migrationBuilder.DropTable(
                name: "EligibilityEnquiry270Master");

            migrationBuilder.DropTable(
                name: "EligibilityEnquiryServiceTypeMaster");
        }
    }
}
