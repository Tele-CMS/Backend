using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddTableGroupSessionInvitations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupSessionInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    InvitaionId = table.Column<Guid>(nullable: true, defaultValueSql: "NEWID()"),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<int>(nullable: true),
                    SessionId = table.Column<int>(nullable: true),
                    SessionMode = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSessionInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_PatientAppointment_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "PatientAppointment",
                        principalColumn: "PatientAppointmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_TelehealthSessionDetails_SessionId",
                        column: x => x.SessionId,
                        principalTable: "TelehealthSessionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupSessionInvitations_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_AppointmentId",
                table: "GroupSessionInvitations",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_CreatedBy",
                table: "GroupSessionInvitations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_DeletedBy",
                table: "GroupSessionInvitations",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_OrganizationId",
                table: "GroupSessionInvitations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_SessionId",
                table: "GroupSessionInvitations",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_UpdatedBy",
                table: "GroupSessionInvitations",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupSessionInvitations");
        }
    }
}
