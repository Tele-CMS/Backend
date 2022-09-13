using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class QuestionnaireDB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                table: "DFA_CategoryCode");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "DFA_CategoryCode",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "DFA_Category",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "DFA_CategoryCode");

            migrationBuilder.AddColumn<int>(
                name: "Display",
                table: "DFA_CategoryCode",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "DFA_Category",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
