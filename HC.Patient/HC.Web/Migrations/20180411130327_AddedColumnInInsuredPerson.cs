using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedColumnInInsuredPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "InsuredPerson",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "InsuredPerson",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "InsuredPerson");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "InsuredPerson");
        }
    }
}
