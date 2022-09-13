using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class testMapping1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationProcedures_TypeID",
                table: "AuthorizationProcedures",
                column: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationProcedures_GlobalCode_TypeID",
                table: "AuthorizationProcedures",
                column: "TypeID",
                principalTable: "GlobalCode",
                principalColumn: "GlobalCodeID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationProcedures_GlobalCode_TypeID",
                table: "AuthorizationProcedures");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationProcedures_TypeID",
                table: "AuthorizationProcedures");
        }
    }
}
