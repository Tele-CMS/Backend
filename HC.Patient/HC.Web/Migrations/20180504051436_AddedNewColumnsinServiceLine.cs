using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnsinServiceLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBillableEncounter",
                table: "PatientEncounter",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PatientEncounter",
                type: "varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateModifier1",
                table: "ClaimServiceLine",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateModifier2",
                table: "ClaimServiceLine",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateModifier3",
                table: "ClaimServiceLine",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RateModifier4",
                table: "ClaimServiceLine",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBillableEncounter",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PatientEncounter");

            migrationBuilder.DropColumn(
                name: "RateModifier1",
                table: "ClaimServiceLine");

            migrationBuilder.DropColumn(
                name: "RateModifier2",
                table: "ClaimServiceLine");

            migrationBuilder.DropColumn(
                name: "RateModifier3",
                table: "ClaimServiceLine");

            migrationBuilder.DropColumn(
                name: "RateModifier4",
                table: "ClaimServiceLine");
        }
    }
}
