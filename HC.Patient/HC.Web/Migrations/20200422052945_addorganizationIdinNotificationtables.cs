using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class addorganizationIdinNotificationtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "NotificationTypeMappings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "MasterNotificationTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationID",
                table: "MasterNotificationActionTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypeMappings_OrganizationID",
                table: "NotificationTypeMappings",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterNotificationTypes_OrganizationID",
                table: "MasterNotificationTypes",
                column: "OrganizationID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterNotificationActionTypes_OrganizationID",
                table: "MasterNotificationActionTypes",
                column: "OrganizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterNotificationActionTypes_Organization_OrganizationID",
                table: "MasterNotificationActionTypes",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MasterNotificationTypes_Organization_OrganizationID",
                table: "MasterNotificationTypes",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationTypeMappings_Organization_OrganizationID",
                table: "NotificationTypeMappings",
                column: "OrganizationID",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterNotificationActionTypes_Organization_OrganizationID",
                table: "MasterNotificationActionTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_MasterNotificationTypes_Organization_OrganizationID",
                table: "MasterNotificationTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationTypeMappings_Organization_OrganizationID",
                table: "NotificationTypeMappings");

            migrationBuilder.DropIndex(
                name: "IX_NotificationTypeMappings_OrganizationID",
                table: "NotificationTypeMappings");

            migrationBuilder.DropIndex(
                name: "IX_MasterNotificationTypes_OrganizationID",
                table: "MasterNotificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_MasterNotificationActionTypes_OrganizationID",
                table: "MasterNotificationActionTypes");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "NotificationTypeMappings");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "MasterNotificationTypes");

            migrationBuilder.DropColumn(
                name: "OrganizationID",
                table: "MasterNotificationActionTypes");
        }
    }
}
