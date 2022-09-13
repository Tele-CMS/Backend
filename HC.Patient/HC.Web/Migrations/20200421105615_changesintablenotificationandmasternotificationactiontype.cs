using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class changesintablenotificationandmasternotificationactiontype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MasterNotificationActionTypes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Notifications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubTypeId",
                table: "MasterNotificationActionTypes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "MasterNotificationActionTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "SubTypeId",
                table: "MasterNotificationActionTypes");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "MasterNotificationActionTypes");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "MasterNotificationActionTypes",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
