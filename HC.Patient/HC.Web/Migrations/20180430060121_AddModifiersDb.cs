using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddModifiersDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterServiceCodeModifiers",
                columns: table => new
                {
                    ModifierID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    Modifier = table.Column<string>(nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    ServiceCodeID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterServiceCodeModifiers", x => x.ModifierID);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_MasterServiceCode_ServiceCodeID",
                        column: x => x.ServiceCodeID,
                        principalTable: "MasterServiceCode",
                        principalColumn: "ServiceCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_CreatedBy",
                table: "MasterServiceCodeModifiers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_DeletedBy",
                table: "MasterServiceCodeModifiers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_ServiceCodeID",
                table: "MasterServiceCodeModifiers",
                column: "ServiceCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_UpdatedBy",
                table: "MasterServiceCodeModifiers",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterServiceCodeModifiers");
        }
    }
}
