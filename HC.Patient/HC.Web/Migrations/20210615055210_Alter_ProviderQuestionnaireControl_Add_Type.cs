using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Alter_ProviderQuestionnaireControl_Add_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ProviderQuestionnaireControls",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotify",
                table: "PatientAppointment",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ProviderQuestionnaireControls");

            migrationBuilder.DropColumn(
                name: "IsNotify",
                table: "PatientAppointment");
        }
    }
}
