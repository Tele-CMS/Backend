using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AdUserRoleInUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "UserInvitation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_RoleId",
                table: "UserInvitation",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_UserRoles_RoleId",
                table: "UserInvitation",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_UserRoles_RoleId",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_RoleId",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "UserInvitation");
        }
    }
}
