using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddnewColumns_UserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApnToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TempIdGeneratedTime",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TempResetPasswordId",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApnToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TempIdGeneratedTime",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TempResetPasswordId",
                table: "User");
        }
    }
}
