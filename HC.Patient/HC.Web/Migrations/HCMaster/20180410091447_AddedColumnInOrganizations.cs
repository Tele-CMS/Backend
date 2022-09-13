using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class AddedColumnInOrganizations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorIdDirect",
                table: "MasterOrganization",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorIdIndirect",
                table: "MasterOrganization",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorNameDirect",
                table: "MasterOrganization",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VendorNameIndirect",
                table: "MasterOrganization",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorIdDirect",
                table: "MasterOrganization");

            migrationBuilder.DropColumn(
                name: "VendorIdIndirect",
                table: "MasterOrganization");

            migrationBuilder.DropColumn(
                name: "VendorNameDirect",
                table: "MasterOrganization");

            migrationBuilder.DropColumn(
                name: "VendorNameIndirect",
                table: "MasterOrganization");
        }
    }
}
