using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedTableAuthProcedureCPTModifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthProcedureCPTModifiers",
                columns: table => new
                {
                    AuthProcedureCPTModifierLinkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthProcedureCPTLinkId = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true, defaultValueSql: "GetUtcDate()"),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Modifier = table.Column<string>(maxLength: 20, nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthProcedureCPTModifiers", x => x.AuthProcedureCPTModifierLinkId);
                    table.ForeignKey(
                        name: "FK_AuthProcedureCPTModifiers_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthProcedureCPTModifiers_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuthProcedureCPTModifiers_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthProcedureCPTModifiers_CreatedBy",
                table: "AuthProcedureCPTModifiers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProcedureCPTModifiers_DeletedBy",
                table: "AuthProcedureCPTModifiers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuthProcedureCPTModifiers_UpdatedBy",
                table: "AuthProcedureCPTModifiers",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthProcedureCPTModifiers");
        }
    }
}
