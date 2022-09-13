using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MessageSystemDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    FromPatientId = table.Column<int>(nullable: true),
                    FromStaffId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    IsFavourite = table.Column<bool>(nullable: false, defaultValue: false),
                    MessageDate = table.Column<DateTime>(nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Message_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Patients_FromPatientId",
                        column: x => x.FromPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Staffs_FromStaffId",
                        column: x => x.FromStaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageRecepient",
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
                    MessageDate = table.Column<DateTime>(nullable: false),
                    MessageId = table.Column<int>(nullable: false),
                    ToPatientId = table.Column<int>(nullable: true),
                    ToStaffId = table.Column<int>(nullable: true),
                    Unread = table.Column<bool>(nullable: false, defaultValue: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRecepient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_Patients_ToPatientId",
                        column: x => x.ToPatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_Staffs_ToStaffId",
                        column: x => x.ToStaffId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageRecepient_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_CreatedBy",
                table: "Message",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Message_DeletedBy",
                table: "Message",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromPatientId",
                table: "Message",
                column: "FromPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromStaffId",
                table: "Message",
                column: "FromStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_OrganizationId",
                table: "Message",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UpdatedBy",
                table: "Message",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_CreatedBy",
                table: "MessageRecepient",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_DeletedBy",
                table: "MessageRecepient",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_MessageId",
                table: "MessageRecepient",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_ToPatientId",
                table: "MessageRecepient",
                column: "ToPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_ToStaffId",
                table: "MessageRecepient",
                column: "ToStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_UpdatedBy",
                table: "MessageRecepient",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageRecepient");

            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
