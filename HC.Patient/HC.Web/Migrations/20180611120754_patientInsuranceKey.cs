using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class patientInsuranceKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsuredPerson_PatientInsuranceID",
                table: "InsuredPerson");

            migrationBuilder.CreateIndex(
                name: "IX_InsuredPerson_PatientInsuranceID",
                table: "InsuredPerson",
                column: "PatientInsuranceID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InsuredPerson_PatientInsuranceID",
                table: "InsuredPerson");

            migrationBuilder.CreateIndex(
                name: "IX_InsuredPerson_PatientInsuranceID",
                table: "InsuredPerson",
                column: "PatientInsuranceID");
        }
    }
}
