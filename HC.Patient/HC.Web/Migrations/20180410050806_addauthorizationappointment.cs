using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class addauthorizationappointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentAuthorization",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(nullable: false),
                    AuthProcedureCPTLinkId = table.Column<int>(nullable: false),
                    AuthScheduledDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ServiceCodeId = table.Column<int>(nullable: false),
                    UnitsBlocked = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentAuthorization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_PatientAppointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "PatientAppointment",
                        principalColumn: "PatientAppointmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_AuthProcedureCPT_AuthProcedureCPTLinkId",
                        column: x => x.AuthProcedureCPTLinkId,
                        principalTable: "AuthProcedureCPT",
                        principalColumn: "AuthProcedureCPTLinkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_MasterServiceCode_ServiceCodeId",
                        column: x => x.ServiceCodeId,
                        principalTable: "MasterServiceCode",
                        principalColumn: "ServiceCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentAuthorization_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_AppointmentId",
                table: "AppointmentAuthorization",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_AuthProcedureCPTLinkId",
                table: "AppointmentAuthorization",
                column: "AuthProcedureCPTLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_CreatedBy",
                table: "AppointmentAuthorization",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_DeletedBy",
                table: "AppointmentAuthorization",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_ServiceCodeId",
                table: "AppointmentAuthorization",
                column: "ServiceCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentAuthorization_UpdatedBy",
                table: "AppointmentAuthorization",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentAuthorization");
        }
    }
}
