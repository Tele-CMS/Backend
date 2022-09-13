using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class createpwdhistory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "varchar(50)",
                table: "UserPasswordHistory",
                newName: "Password");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "UserPasswordHistory",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "UserPasswordHistory",
                newName: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "varchar(50)",
                table: "UserPasswordHistory",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);
        }
    }
}
