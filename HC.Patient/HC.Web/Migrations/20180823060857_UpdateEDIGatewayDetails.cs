using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class UpdateEDIGatewayDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Path999",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path837",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path835",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path277",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path270",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path271",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path276",
                table: "EDIGateway",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path270",
                table: "EDIGateway");

            migrationBuilder.DropColumn(
                name: "Path271",
                table: "EDIGateway");

            migrationBuilder.DropColumn(
                name: "Path276",
                table: "EDIGateway");

            migrationBuilder.AlterColumn<string>(
                name: "Path999",
                table: "EDIGateway",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path837",
                table: "EDIGateway",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path835",
                table: "EDIGateway",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path277",
                table: "EDIGateway",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
