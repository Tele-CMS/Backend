using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MessageChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_DeletedBy",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UpdatedBy",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_DeletedBy",
                table: "MessageRecepient");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_UpdatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_CreatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_DeletedBy",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_UpdatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_Message_CreatedBy",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_DeletedBy",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_UpdatedBy",
                table: "Message");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_CreatedBy",
                table: "MessageRecepient",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_DeletedBy",
                table: "MessageRecepient",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_UpdatedBy",
                table: "MessageRecepient",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Message_CreatedBy",
                table: "Message",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Message_DeletedBy",
                table: "Message",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UpdatedBy",
                table: "Message",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_DeletedBy",
                table: "Message",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UpdatedBy",
                table: "Message",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_DeletedBy",
                table: "MessageRecepient",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_UpdatedBy",
                table: "MessageRecepient",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
