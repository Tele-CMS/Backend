using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MakeUserInvitationIdNullableInInvitationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation");

            migrationBuilder.AlterColumn<int>(
                name: "InvitedUserId",
                table: "UserInvitation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation",
                column: "InvitedUserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation");

            migrationBuilder.AlterColumn<int>(
                name: "InvitedUserId",
                table: "UserInvitation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_InvitedUserId",
                table: "UserInvitation",
                column: "InvitedUserId",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
