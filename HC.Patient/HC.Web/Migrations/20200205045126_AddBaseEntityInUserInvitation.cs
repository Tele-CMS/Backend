using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddBaseEntityInUserInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserInvitation",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserInvitation",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserInvitation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserInvitation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "UserInvitation",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_CreatedBy",
                table: "UserInvitation",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_DeletedBy",
                table: "UserInvitation",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvitation_UpdatedBy",
                table: "UserInvitation",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_CreatedBy",
                table: "UserInvitation",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_DeletedBy",
                table: "UserInvitation",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInvitation_User_UpdatedBy",
                table: "UserInvitation",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_CreatedBy",
                table: "UserInvitation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_DeletedBy",
                table: "UserInvitation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInvitation_User_UpdatedBy",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_CreatedBy",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_DeletedBy",
                table: "UserInvitation");

            migrationBuilder.DropIndex(
                name: "IX_UserInvitation_UpdatedBy",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserInvitation");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "UserInvitation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserInvitation",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "UserInvitation",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
