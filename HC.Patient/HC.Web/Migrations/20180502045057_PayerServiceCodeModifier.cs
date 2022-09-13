using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class PayerServiceCodeModifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "ServiceCode",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.RenameColumn(
                name: "ModifierId",
                table: "PayerServiceCodeModifiers",
                newName: "PayerModifierId");

            migrationBuilder.AlterColumn<string>(
                name: "Modifier",
                table: "PayerServiceCodeModifiers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "PayerServiceCodeModifiers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "PayerServiceCodeModifiers",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "PayerServiceCodeModifiers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PayerServiceCodeModifiers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PayerServiceCodeModifiers",
                type: "BIT",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PayerServiceCodeModifiers",
                type: "BIT",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "PayerServiceCodeModifiers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "PayerServiceCodeModifiers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayerServiceCodeModifiers_CreatedBy",
                table: "PayerServiceCodeModifiers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayerServiceCodeModifiers_DeletedBy",
                table: "PayerServiceCodeModifiers",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PayerServiceCodeModifiers_UpdatedBy",
                table: "PayerServiceCodeModifiers",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_CreatedBy",
                table: "PayerServiceCodeModifiers",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_DeletedBy",
                table: "PayerServiceCodeModifiers",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_UpdatedBy",
                table: "PayerServiceCodeModifiers",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_CreatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_DeletedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropForeignKey(
                name: "FK_PayerServiceCodeModifiers_User_UpdatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropIndex(
                name: "IX_PayerServiceCodeModifiers_CreatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropIndex(
                name: "IX_PayerServiceCodeModifiers_DeletedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropIndex(
                name: "IX_PayerServiceCodeModifiers_UpdatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "PayerServiceCodeModifiers");

            migrationBuilder.RenameColumn(
                name: "PayerModifierId",
                table: "PayerServiceCodeModifiers",
                newName: "ModifierId");

            migrationBuilder.AlterColumn<string>(
                name: "Modifier",
                table: "PayerServiceCodeModifiers",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PayerServiceCodeModifiers",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceCode",
                table: "PayerServiceCodeModifiers",
                type: "varchar(50)",
                nullable: true);
        }
    }
}
