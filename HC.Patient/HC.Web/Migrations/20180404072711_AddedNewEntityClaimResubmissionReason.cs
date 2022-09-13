using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewEntityClaimResubmissionReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmissionType",
                table: "Claims",
                type: "tinyint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClaimResubmissionReason",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(type: "varchar(200)", nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    ResubmissionCode = table.Column<int>(type: "varchar(4)", nullable: false),
                    ResubmissionReason = table.Column<string>(type: "varchar(200)", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimResubmissionReason", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimResubmissionReason_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResubmissionReason_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimResubmissionReason_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResubmissionReason_CreatedBy",
                table: "ClaimResubmissionReason",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResubmissionReason_DeletedBy",
                table: "ClaimResubmissionReason",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimResubmissionReason_UpdatedBy",
                table: "ClaimResubmissionReason",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimResubmissionReason");

            migrationBuilder.DropColumn(
                name: "SubmissionType",
                table: "Claims");
        }
    }
}
