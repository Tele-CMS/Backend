using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedColumnInOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorIdDirect",
                table: "Organization",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorIdIndirect",
                table: "Organization",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorNameDirect",
                table: "Organization",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorNameIndirect",
                table: "Organization",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorIdDirect",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "VendorIdIndirect",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "VendorNameDirect",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "VendorNameIndirect",
                table: "Organization");
        }
    }
}
