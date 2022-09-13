using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations.HCMaster
{
    public partial class changesOrganizationDatabaseDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "OrganizationDatabaseDetail",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "OrganizationDatabaseDetail",
                nullable: false,
                defaultValueSql: "GetDate()");

            migrationBuilder.AddColumn<int>(
                name: "DeletedBy",
                table: "OrganizationDatabaseDetail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "OrganizationDatabaseDetail",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "OrganizationDatabaseDetail",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCentralised",
                table: "OrganizationDatabaseDetail",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OrganizationDatabaseDetail",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "OrganizationDatabaseDetail",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "OrganizationDatabaseDetail",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "IsCentralised",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OrganizationDatabaseDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "OrganizationDatabaseDetail");
        }
    }
}
