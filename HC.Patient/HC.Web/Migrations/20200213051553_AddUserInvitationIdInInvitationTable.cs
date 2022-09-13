using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddUserInvitationIdInInvitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvitedUserId",
                table: "UserInvitation",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_InvitedUserId",
                table: "UserInvitation",
                column: "InvitedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation",
                column: "InvitedUserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_InvitedUserId",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "InvitedUserId",
                table: "UserInvitation");
        }
    }
}
