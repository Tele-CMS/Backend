using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedTableEDI999AcknowledgementMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EDI999AcknowledgementMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Claim837BatchId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    EDIFileText = table.Column<string>(type: "text", nullable: true),
                    EligibilityEnquiry270MasterId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ResponseType = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EDI999AcknowledgementMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EDI999AcknowledgementMaster_Claim837Batch_Claim837BatchId",
                        column: x => x.Claim837BatchId,
                        principalTable: "Claim837Batch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EDI999AcknowledgementMaster_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EDI999AcknowledgementMaster_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EDI999AcknowledgementMaster_EligibilityEnquiry270Master_EligibilityEnquiry270MasterId",
                        column: x => x.EligibilityEnquiry270MasterId,
                        principalTable: "EligibilityEnquiry270Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EDI999AcknowledgementMaster_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_Claim837BatchId",
                table: "EDI999AcknowledgementMaster",
                column: "Claim837BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_CreatedBy",
                table: "EDI999AcknowledgementMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_DeletedBy",
                table: "EDI999AcknowledgementMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_EligibilityEnquiry270MasterId",
                table: "EDI999AcknowledgementMaster",
                column: "EligibilityEnquiry270MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_EDI999AcknowledgementMaster_UpdatedBy",
                table: "EDI999AcknowledgementMaster",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EDI999AcknowledgementMaster");
        }
    }
}
