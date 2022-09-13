using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class StaffPayrollRateForActivityDb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatePerUnit",
                table: "StaffPayrollRateForActivity",
                newName: "PayRate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayRate",
                table: "StaffPayrollRateForActivity",
                newName: "RatePerUnit");
        }
    }
}
