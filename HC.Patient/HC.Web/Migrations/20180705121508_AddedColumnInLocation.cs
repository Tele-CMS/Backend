using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedColumnInLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "StaffAvailability",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StaffAvailability_LocationId",
                table: "StaffAvailability",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StaffAvailability_Location_LocationId",
                table: "StaffAvailability",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "LocationID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StaffAvailability_Location_LocationId",
                table: "StaffAvailability");

            migrationBuilder.DropIndex(
                name: "IX_StaffAvailability_LocationId",
                table: "StaffAvailability");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "StaffAvailability");
        }
    }
}
