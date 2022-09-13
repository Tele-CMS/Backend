using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class NewChangesInEncounter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsBillableEncounter",
                table: "PatientEncounter",
                nullable: true,
                oldClrType: typeof(bool),
                oldDefaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsBillableEncounter",
                table: "PatientEncounter",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
