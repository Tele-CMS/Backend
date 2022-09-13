using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class StaffPayrollRateForActivityDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaffPayrollRateForActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentTypeId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    RatePerUnit = table.Column<decimal>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffPayrollRateForActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffPayrollRateForActivity_AppointmentType_AppointmentTypeId",
                        column: x => x.AppointmentTypeId,
                        principalTable: "AppointmentType",
                        principalColumn: "AppointmentTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffPayrollRateForActivity_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffPayrollRateForActivity_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffPayrollRateForActivity_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffPayrollRateForActivity_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthProcedureCPTModifiers_AuthProcedureCPTLinkId",
                table: "AuthProcedureCPTModifiers",
                column: "AuthProcedureCPTLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPayrollRateForActivity_AppointmentTypeId",
                table: "StaffPayrollRateForActivity",
                column: "AppointmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPayrollRateForActivity_CreatedBy",
                table: "StaffPayrollRateForActivity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPayrollRateForActivity_DeletedBy",
                table: "StaffPayrollRateForActivity",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPayrollRateForActivity_StaffId",
                table: "StaffPayrollRateForActivity",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPayrollRateForActivity_UpdatedBy",
                table: "StaffPayrollRateForActivity",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthProcedureCPTModifiers_AuthProcedureCPT_AuthProcedureCPTLinkId",
                table: "AuthProcedureCPTModifiers",
                column: "AuthProcedureCPTLinkId",
                principalTable: "AuthProcedureCPT",
                principalColumn: "AuthProcedureCPTLinkId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthProcedureCPTModifiers_AuthProcedureCPT_AuthProcedureCPTLinkId",
                table: "AuthProcedureCPTModifiers");

            migrationBuilder.DropTable(
                name: "StaffPayrollRateForActivity");

            migrationBuilder.DropIndex(
                name: "IX_AuthProcedureCPTModifiers_AuthProcedureCPTLinkId",
                table: "AuthProcedureCPTModifiers");
        }
    }
}
