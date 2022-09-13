using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class updatestatusintimesheet1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_AppointmentType_AppointmentTypeId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropTable(
                name: "UserDetailedDriveTime");

            migrationBuilder.DropTable(
                name: "UserDriveTime");

            migrationBuilder.DropTable(
                name: "UserTimesheet");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeId",
                table: "UserTimesheetByAppointmentType",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<decimal>(
                name: "Distance",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DriveTime",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsTravelTime",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PatientEncounterId",
                table: "UserTimesheetByAppointmentType",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheetByAppointmentType_PatientEncounterId",
                table: "UserTimesheetByAppointmentType",
                column: "PatientEncounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_AppointmentType_AppointmentTypeId",
                table: "UserTimesheetByAppointmentType",
                column: "AppointmentTypeId",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_PatientEncounter_PatientEncounterId",
                table: "UserTimesheetByAppointmentType",
                column: "PatientEncounterId",
                principalTable: "PatientEncounter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_AppointmentType_AppointmentTypeId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTimesheetByAppointmentType_PatientEncounter_PatientEncounterId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropIndex(
                name: "IX_UserTimesheetByAppointmentType_PatientEncounterId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "DriveTime",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "IsTravelTime",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "PatientEncounterId",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "UserTimesheetByAppointmentType");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentTypeId",
                table: "UserTimesheetByAppointmentType",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserDetailedDriveTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Distance = table.Column<decimal>(nullable: false),
                    DriveTime = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PatientEncounterId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    TravelEndTime = table.Column<DateTime>(nullable: false),
                    TravelStartTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetailedDriveTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDetailedDriveTime_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetailedDriveTime_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetailedDriveTime_PatientEncounter_PatientEncounterId",
                        column: x => x.PatientEncounterId,
                        principalTable: "PatientEncounter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDetailedDriveTime_GlobalCode_StatusId",
                        column: x => x.StatusId,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDetailedDriveTime_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDriveTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DateOfService = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    TotalDistance = table.Column<decimal>(nullable: false),
                    TotalDriveTime = table.Column<decimal>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDriveTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDriveTime_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDriveTime_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserDriveTime_Staffs_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDriveTime_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTimesheet",
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
                    PatientEncounterId = table.Column<int>(nullable: true),
                    ServiceDuration = table.Column<decimal>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    UserTimesheetByAppointmentTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTimesheet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_PatientEncounter_PatientEncounterId",
                        column: x => x.PatientEncounterId,
                        principalTable: "PatientEncounter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_GlobalCode_StatusId",
                        column: x => x.StatusId,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserTimesheet_UserTimesheetByAppointmentType_UserTimesheetByAppointmentTypeId",
                        column: x => x.UserTimesheetByAppointmentTypeId,
                        principalTable: "UserTimesheetByAppointmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_CreatedBy",
                table: "UserDetailedDriveTime",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_DeletedBy",
                table: "UserDetailedDriveTime",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_PatientEncounterId",
                table: "UserDetailedDriveTime",
                column: "PatientEncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_StatusId",
                table: "UserDetailedDriveTime",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDetailedDriveTime_UpdatedBy",
                table: "UserDetailedDriveTime",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDriveTime_CreatedBy",
                table: "UserDriveTime",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDriveTime_DeletedBy",
                table: "UserDriveTime",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserDriveTime_StaffId",
                table: "UserDriveTime",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDriveTime_UpdatedBy",
                table: "UserDriveTime",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_CreatedBy",
                table: "UserTimesheet",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_DeletedBy",
                table: "UserTimesheet",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_PatientEncounterId",
                table: "UserTimesheet",
                column: "PatientEncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_StatusId",
                table: "UserTimesheet",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_UpdatedBy",
                table: "UserTimesheet",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserTimesheet_UserTimesheetByAppointmentTypeId",
                table: "UserTimesheet",
                column: "UserTimesheetByAppointmentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTimesheetByAppointmentType_AppointmentType_AppointmentTypeId",
                table: "UserTimesheetByAppointmentType",
                column: "AppointmentTypeId",
                principalTable: "AppointmentType",
                principalColumn: "AppointmentTypeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
