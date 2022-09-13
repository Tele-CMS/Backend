using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddTableAppointmentPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_AppointmentPayments_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_AppointmentPayments_UpdatedBy",
                table: "AppointmentPayments",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentPayments");
        }
    }
}
