using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnsAGESEXTOTableSymptomatePatientReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Age",
                table: "SymptomatePatientReport",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "SymptomatePatientReport",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "SymptomatePatientReport");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "SymptomatePatientReport");
        }
    }
}
