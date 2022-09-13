using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class SectionItemDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFA_SectionItem_DFA_Category_CategoryId",
                table: "DFA_SectionItem");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "DFA_SectionItem",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_DFA_SectionItem_DFA_Category_CategoryId",
                table: "DFA_SectionItem",
                column: "CategoryId",
                principalTable: "DFA_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DFA_SectionItem_DFA_Category_CategoryId",
                table: "DFA_SectionItem");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "DFA_SectionItem",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DFA_SectionItem_DFA_Category_CategoryId",
                table: "DFA_SectionItem",
                column: "CategoryId",
                principalTable: "DFA_Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
