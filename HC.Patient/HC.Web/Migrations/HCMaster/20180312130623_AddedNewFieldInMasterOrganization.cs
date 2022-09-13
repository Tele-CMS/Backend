using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class AddedNewFieldInMasterOrganization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "MasterOrganization",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "MasterOrganization",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "MasterOrganization");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "MasterOrganization");
        }
    }
}
