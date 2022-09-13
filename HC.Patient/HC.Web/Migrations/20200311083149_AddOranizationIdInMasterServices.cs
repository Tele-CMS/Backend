using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddOranizationIdInMasterServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MasterServices",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                table: "MasterServices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MasterServices_OrganizationId",
                table: "MasterServices",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterServices_Organization_OrganizationId",
                table: "MasterServices",
                column: "OrganizationId",
                principalTable: "Organization",
                principalColumn: "OrganizationID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterServices_Organization_OrganizationId",
                table: "MasterServices");

            migrationBuilder.DropIndex(
                name: "IX_MasterServices_OrganizationId",
                table: "MasterServices");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "MasterServices");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceName",
                table: "MasterServices",
                type: "varchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)");
        }
    }
}
