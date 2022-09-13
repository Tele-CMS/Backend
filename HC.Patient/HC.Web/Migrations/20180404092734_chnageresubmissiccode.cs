using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class chnageresubmissiccode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResubmissionCode",
                table: "ClaimResubmissionReason",
                type: "varchar(4)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "varchar(4)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResubmissionCode",
                table: "ClaimResubmissionReason",
                type: "varchar(4)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(4)",
                oldNullable: true);
        }
    }
}
