using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ToUserIdNullAndRoomIdInChatTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ToUserId",
                table: "Chat",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Chat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Chat");

            migrationBuilder.AlterColumn<int>(
                name: "ToUserId",
                table: "Chat",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
