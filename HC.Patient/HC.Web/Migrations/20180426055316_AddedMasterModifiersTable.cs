using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedMasterModifiersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterServiceCodeModifiers");

            migrationBuilder.CreateTable(
                name: "MasterModifiers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Modifier = table.Column<string>(type: "varchar(5)", nullable: true),
                    OrganizationID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterModifiers_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterModifiers_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterModifiers_Organization_OrganizationID",
                        column: x => x.OrganizationID,
                        principalTable: "Organization",
                        principalColumn: "OrganizationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterModifiers_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterModifiers_CreatedBy",
                table: "MasterModifiers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterModifiers_DeletedBy",
                table: "MasterModifiers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterModifiers_OrganizationID",
                table: "MasterModifiers",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterModifiers_UpdatedBy",
                table: "MasterModifiers",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterModifiers");

            migrationBuilder.CreateTable(
                name: "MasterServiceCodeModifiers",
                columns: table => new
                {
                    ModifierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Modifier = table.Column<string>(type: "varchar(20)", nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    ServiceCode = table.Column<string>(type: "varchar(50)", nullable: true),
                    ServiceCodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterServiceCodeModifiers", x => x.ModifierId);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_MasterServiceCode_ServiceCodeId",
                        column: x => x.ServiceCodeId,
                        principalTable: "MasterServiceCode",
                        principalColumn: "ServiceCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_ServiceCodeId",
                table: "MasterServiceCodeModifiers",
                column: "ServiceCodeId");
        }
    }
}
