using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddUserIdInGroupInvitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "GroupSessionInvitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupSessionInvitations_UserID",
                table: "GroupSessionInvitations",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSessionInvitations_User_UserID",
                table: "GroupSessionInvitations",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupSessionInvitations_User_UserID",
                table: "GroupSessionInvitations");

            migrationBuilder.DropIndex(
                name: "IX_GroupSessionInvitations_UserID",
                table: "GroupSessionInvitations");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "GroupSessionInvitations");
        }
    }
}
