using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnGlobalCodeIdToGlobalCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GlobalCodeId",
                table: "MasterServices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MasterServices_GlobalCodeId",
                table: "MasterServices",
                column: "GlobalCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterServices_GlobalCode_GlobalCodeId",
                table: "MasterServices",
                column: "GlobalCodeId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterServices_GlobalCode_GlobalCodeId",
                table: "MasterServices");

            migrationBuilder.DropIndex(
                name: "IX_MasterServices_GlobalCodeId",
                table: "MasterServices");

            migrationBuilder.DropColumn(
                name: "GlobalCodeId",
                table: "MasterServices");
        }
    }
}
