using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class testMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationProcedures_GlobalCode_GlobalCodeId",
                table: "AuthorizationProcedures");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationProcedures_GlobalCodeId",
                table: "AuthorizationProcedures");

            migrationBuilder.DropColumn(
                name: "GlobalCodeId",
                table: "AuthorizationProcedures");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GlobalCodeId",
                table: "AuthorizationProcedures",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationProcedures_GlobalCodeId",
                table: "AuthorizationProcedures",
                column: "GlobalCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationProcedures_GlobalCode_GlobalCodeId",
                table: "AuthorizationProcedures",
                column: "GlobalCodeId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
