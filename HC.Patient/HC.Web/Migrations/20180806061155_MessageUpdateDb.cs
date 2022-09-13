using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MessageUpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentMessageId",
                table: "Message",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ParentMessageId",
                table: "Message",
                column: "ParentMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Message_ParentMessageId",
                table: "Message",
                column: "ParentMessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Message_ParentMessageId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ParentMessageId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ParentMessageId",
                table: "Message");
        }
    }
}
