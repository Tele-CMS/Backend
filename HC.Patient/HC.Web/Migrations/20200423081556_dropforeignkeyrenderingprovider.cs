using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class dropforeignkeyrenderingprovider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Staffs_RenderingProviderID",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_RenderingProviderID",
                table: "Patients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Patients_RenderingProviderID",
                table: "Patients",
                column: "RenderingProviderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Staffs_RenderingProviderID",
                table: "Patients",
                column: "RenderingProviderID",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
