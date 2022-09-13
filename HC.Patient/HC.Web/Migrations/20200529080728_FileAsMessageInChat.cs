using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class FileAsMessageInChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Chat",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                table: "Chat",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "Chat",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "FileType",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Chat");
        }
    }
}
