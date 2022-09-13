using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddRefundTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefundToken",
                table: "AppointmentPayments");

            migrationBuilder.CreateTable(
                name: "AppointmentPaymentRefund",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppointmentId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PaymentToken = table.Column<string>(nullable: false),
                    RefundToken = table.Column<string>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentPaymentRefund", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentPaymentRefund_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentPaymentRefund_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentPaymentRefund_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPaymentRefund_CreatedBy",
                table: "AppointmentPaymentRefund",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPaymentRefund_DeletedBy",
                table: "AppointmentPaymentRefund",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentPaymentRefund_UpdatedBy",
                table: "AppointmentPaymentRefund",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentPaymentRefund");

            migrationBuilder.AddColumn<string>(
                name: "RefundToken",
                table: "AppointmentPayments",
                nullable: true);
        }
    }
}
