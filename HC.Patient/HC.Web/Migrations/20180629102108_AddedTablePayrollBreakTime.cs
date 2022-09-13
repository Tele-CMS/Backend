using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedTablePayrollBreakTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterPayrollBreakTime");

            migrationBuilder.CreateTable(
                name: "PayrollBreakTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    OrganizationId = table.Column<int>(nullable: false),
                    StateId = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollBreakTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_MasterState_StateId",
                        column: x => x.StateId,
                        principalTable: "MasterState",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTime_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayrollBreakTimeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EndRange = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NumberOfBreaks = table.Column<int>(nullable: false),
                    PayrollBreakTimeId = table.Column<int>(nullable: false),
                    StartRange = table.Column<decimal>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollBreakTimeDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_PayrollBreakTime_PayrollBreakTimeId",
                        column: x => x.PayrollBreakTimeId,
                        principalTable: "PayrollBreakTime",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollBreakTimeDetails_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_CreatedBy",
                table: "PayrollBreakTime",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_DeletedBy",
                table: "PayrollBreakTime",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_OrganizationId",
                table: "PayrollBreakTime",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_StateId",
                table: "PayrollBreakTime",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTime_UpdatedBy",
                table: "PayrollBreakTime",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_CreatedBy",
                table: "PayrollBreakTimeDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_DeletedBy",
                table: "PayrollBreakTimeDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_PayrollBreakTimeId",
                table: "PayrollBreakTimeDetails",
                column: "PayrollBreakTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollBreakTimeDetails_UpdatedBy",
                table: "PayrollBreakTimeDetails",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollBreakTimeDetails");

            migrationBuilder.DropTable(
                name: "PayrollBreakTime");

            migrationBuilder.CreateTable(
                name: "MasterPayrollBreakTime",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BreakDuration = table.Column<decimal>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EndRange = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NumberOfBreaks = table.Column<int>(nullable: false),
                    StartRange = table.Column<decimal>(nullable: false),
                    StateAbbr = table.Column<string>(type: "varchar(5)", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterPayrollBreakTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterPayrollBreakTime_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterPayrollBreakTime_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterPayrollBreakTime_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterPayrollBreakTime_CreatedBy",
                table: "MasterPayrollBreakTime",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPayrollBreakTime_DeletedBy",
                table: "MasterPayrollBreakTime",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPayrollBreakTime_UpdatedBy",
                table: "MasterPayrollBreakTime",
                column: "UpdatedBy");
        }
    }
}
