using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedFieldTemplateIdInMasterTemplate_medha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TemplateTypeId",
                table: "MasterTemplates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplates_TemplateTypeId",
                table: "MasterTemplates",
                column: "TemplateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTemplates_GlobalCode_TemplateTypeId",
                table: "MasterTemplates",
                column: "TemplateTypeId",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTemplates_GlobalCode_TemplateTypeId",
                table: "MasterTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MasterTemplates_TemplateTypeId",
                table: "MasterTemplates");

            migrationBuilder.DropColumn(
                name: "TemplateTypeId",
                table: "MasterTemplates");
        }
    }
}
