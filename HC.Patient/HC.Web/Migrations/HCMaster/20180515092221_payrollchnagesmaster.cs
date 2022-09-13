using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class payrollchnagesmaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayrollEndWeekDay",
                table: "MasterOrganization",
                type: "varchar(15)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayrollStartWeekDay",
                table: "MasterOrganization",
                type: "varchar(15)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayrollEndWeekDay",
                table: "MasterOrganization");

            migrationBuilder.DropColumn(
                name: "PayrollStartWeekDay",
                table: "MasterOrganization");
        }
    }
}
