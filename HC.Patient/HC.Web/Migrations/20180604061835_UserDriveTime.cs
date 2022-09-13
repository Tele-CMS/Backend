using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UserDriveTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDetailedDriveTime");

            migrationBuilder.DropTable(
                name: "UserDriveTime");
        }
    }
}
