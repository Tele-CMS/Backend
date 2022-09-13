using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class payrollchnages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Patients",
                nullable: false,
                defaultValueSql: "GetUTCDate()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GetDate()");

            migrationBuilder.AddColumn<string>(
                name: "PayrollEndWeekDay",
                table: "Organization",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PayrollStartWeekDay",
                table: "Organization",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayrollEndWeekDay",
                table: "Organization");

            migrationBuilder.DropColumn(
                name: "PayrollStartWeekDay",
                table: "Organization");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Patients",
                nullable: false,
                defaultValueSql: "GetDate()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GetUTCDate()");
        }
    }
}
