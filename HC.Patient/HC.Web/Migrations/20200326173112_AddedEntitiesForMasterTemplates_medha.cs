using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedEntitiesForMasterTemplates_medha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTemplates_GlobalCode_TemplateTypeId",
                table: "MasterTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment");

            migrationBuilder.RenameColumn(
                name: "TemplateTypeId",
                table: "MasterTemplates",
                newName: "TemplateSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterTemplates_TemplateTypeId",
                table: "MasterTemplates",
                newName: "IX_MasterTemplates_TemplateSubCategoryId");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeID",
                table: "PatientAppointment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "BookingCommision",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMode",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeKey",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "MasterTemplates",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemplateCategoryId",
                table: "MasterTemplates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppointmentPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(nullable: false),
                    BookingAmount = table.Column<decimal>(nullable: false),
                    CommissionPercentage = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    PaymentMode = table.Column<string>(nullable: false),
                    PaymentToken = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentPayments_PatientAppointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "PatientAppointment",
                        principalColumn: "PatientAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentPayments_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentPayments_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentPayments_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentPayments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterTemplateCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    MasterCategoryName = table.Column<string>(nullable: true),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTemplateCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTemplateCategory_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterTemplateCategory_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterTemplateCategory_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterTemplateCategory_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterTemplateSubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DisplayOrder = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    MasterSubCategoryName = table.Column<string>(nullable: true),
                    MasterSubCategoryValue = table.Column<string>(nullable: true),
                    MasterTemplateCategoryId = table.Column<int>(nullable: false),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterTemplateSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterTemplateSubCategory_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterTemplateSubCategory_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterTemplateSubCategory_MasterTemplateCategory_MasterTemplateCategoryId",
                        column: x => x.MasterTemplateCategoryId,
                        principalTable: "MasterTemplateCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterTemplateSubCategory_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterTemplateSubCategory_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplates_TemplateCategoryId",
                table: "MasterTemplates",
                column: "TemplateCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_AppointmentId",
                table: "AppointmentPayments",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_CreatedBy",
                table: "AppointmentPayments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_DeletedBy",
                table: "AppointmentPayments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_OrganizationId",
                table: "AppointmentPayments",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPayments_UpdatedBy",
                table: "AppointmentPayments",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateCategory_CreatedBy",
                table: "MasterTemplateCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateCategory_DeletedBy",
                table: "MasterTemplateCategory",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateCategory_OrganizationID",
                table: "MasterTemplateCategory",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateCategory_UpdatedBy",
                table: "MasterTemplateCategory",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateSubCategory_CreatedBy",
                table: "MasterTemplateSubCategory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateSubCategory_DeletedBy",
                table: "MasterTemplateSubCategory",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateSubCategory_MasterTemplateCategoryId",
                table: "MasterTemplateSubCategory",
                column: "MasterTemplateCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateSubCategory_OrganizationID",
                table: "MasterTemplateSubCategory",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplateSubCategory_UpdatedBy",
                table: "MasterTemplateSubCategory",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTemplates_MasterTemplateCategory_TemplateCategoryId",
                table: "MasterTemplates",
                column: "TemplateCategoryId",
                principalTable: "MasterTemplateCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTemplates_MasterTemplateSubCategory_TemplateSubCategoryId",
                table: "MasterTemplates",
                column: "TemplateSubCategoryId",
                principalTable: "MasterTemplateSubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment",
                column: "AppointmentTypeID",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTemplates_MasterTemplateCategory_TemplateCategoryId",
                table: "MasterTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterTemplates_MasterTemplateSubCategory_TemplateSubCategoryId",
                table: "MasterTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment");

            migrationBuilder.DropTable(
                name: "AppointmentPayments");

            migrationBuilder.DropTable(
                name: "MasterTemplateSubCategory");

            migrationBuilder.DropTable(
                name: "MasterTemplateCategory");

            migrationBuilder.DropIndex(
                name: "IX_MasterTemplates_TemplateCategoryId",
                table: "MasterTemplates");

            migrationBuilder.DropColumn(
                name: "BookingCommision",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "PaymentMode",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "StripeKey",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "MasterTemplates");

            migrationBuilder.DropColumn(
                name: "TemplateCategoryId",
                table: "MasterTemplates");

            migrationBuilder.RenameColumn(
                name: "TemplateSubCategoryId",
                table: "MasterTemplates",
                newName: "TemplateTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_MasterTemplates_TemplateSubCategoryId",
                table: "MasterTemplates",
                newName: "IX_MasterTemplates_TemplateTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeID",
                table: "PatientAppointment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTemplates_GlobalCode_TemplateTypeId",
                table: "MasterTemplates",
                column: "TemplateTypeId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAppointment_AppointmentType_AppointmentTypeID",
                table: "PatientAppointment",
                column: "AppointmentTypeID",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
