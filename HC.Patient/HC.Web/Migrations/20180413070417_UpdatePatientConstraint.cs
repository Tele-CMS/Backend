using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdatePatientConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_GlobalCode_EmergencyContactRelationship",
                table: "Patients");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MasterRelationship_EmergencyContactRelationship",
                table: "Patients",
                column: "EmergencyContactRelationship",
                principalTable: "MasterRelationship",
                principalColumn: "RelationshipID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MasterRelationship_EmergencyContactRelationship",
                table: "Patients");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_GlobalCode_EmergencyContactRelationship",
                table: "Patients",
                column: "EmergencyContactRelationship",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
