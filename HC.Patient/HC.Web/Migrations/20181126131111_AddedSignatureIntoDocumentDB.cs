using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedSignatureIntoDocumentDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ClinicianSign",
                table: "DFA_PatientDocuments",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PatientSign",
                table: "DFA_PatientDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicianSign",
                table: "DFA_PatientDocuments");

            migrationBuilder.DropColumn(
                name: "PatientSign",
                table: "DFA_PatientDocuments");
        }
    }
}
