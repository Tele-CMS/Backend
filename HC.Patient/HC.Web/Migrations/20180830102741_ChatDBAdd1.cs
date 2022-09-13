using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ChatDBAdd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "Chat",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_OrganizationID",
                table: "Chat",
                column: "OrganizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_Organization_OrganizationID",
                table: "Chat",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_Organization_OrganizationID",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_OrganizationID",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "Chat");
        }
    }
}
