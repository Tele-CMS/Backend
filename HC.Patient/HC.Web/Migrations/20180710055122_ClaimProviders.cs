using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ClaimProviders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimProviders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimId = table.Column<int>(nullable: false),
                    ClinicianId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RenderingProviderId = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_Staffs_ClinicianId",
                        column: x => x.ClinicianId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_Staffs_RenderingProviderId",
                        column: x => x.RenderingProviderId,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimProviders_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_ClaimId",
                table: "ClaimProviders",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_ClinicianId",
                table: "ClaimProviders",
                column: "ClinicianId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_CreatedBy",
                table: "ClaimProviders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_DeletedBy",
                table: "ClaimProviders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_RenderingProviderId",
                table: "ClaimProviders",
                column: "RenderingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimProviders_UpdatedBy",
                table: "ClaimProviders",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimProviders");
        }
    }
}
