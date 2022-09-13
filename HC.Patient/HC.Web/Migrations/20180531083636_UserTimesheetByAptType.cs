using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UserTimesheetByAptType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTimesheetByAppointmentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentTypeId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DateOfService = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    TotalDurationForDay = table.Column<decimal>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTimesheetByAppointmentType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTimesheetByAppointmentType_AppointmentType_AppointmentTypeId",
                        column: x => x.AppointmentTypeId,
                        principalTable: "AppointmentType",
                        principalColumn: "AppointmentTypeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTimesheetByAppointmentType_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheetByAppointmentType_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheetByAppointmentType_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTimesheetByAppointmentType_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_PatientEncounterId",
                table: "UserTimesheet",
                column: "PatientEncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet",
                column: "UserTimesheetByAppointmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_AppointmentTypeId",
                table: "UserTimesheetByAppointmentType",
                column: "AppointmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_CreatedBy",
                table: "UserTimesheetByAppointmentType",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_DeletedBy",
                table: "UserTimesheetByAppointmentType",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_StaffId",
                table: "UserTimesheetByAppointmentType",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_UpdatedBy",
                table: "UserTimesheetByAppointmentType",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheet_PatientEncounter_PatientEncounterId",
                table: "UserTimesheet",
                column: "PatientEncounterId",
                principalTable: "PatientEncounter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheet_UserTimesheetByAppointmentType_UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet",
                column: "UserTimesheetByAppointmentTypeId",
                principalTable: "UserTimesheetByAppointmentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheet_PatientEncounter_PatientEncounterId",
                table: "UserTimesheet");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheet_UserTimesheetByAppointmentType_UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet");

            migrationBuilder.DropTable(
                name: "UserTimesheetByAppointmentType");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheet_PatientEncounterId",
                table: "UserTimesheet");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheet_UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet");

            migrationBuilder.DropColumn(
                name: "UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet");
        }
    }
}
