using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class masterTemplatess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "MasterTemplates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MasterTemplates_OrganizationID",
                table: "MasterTemplates",
                column: "OrganizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterTemplates_Organization_OrganizationID",
                table: "MasterTemplates",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterTemplates_Organization_OrganizationID",
                table: "MasterTemplates");

            migrationBuilder.DropIndex(
                name: "IX_MasterTemplates_OrganizationID",
                table: "MasterTemplates");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "MasterTemplates");
        }
    }
}
