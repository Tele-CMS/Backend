using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class ProviderCancellationRulesTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FollowUpDays",
                table: "Staffs",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FollowUpPayRate",
                table: "Staffs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProviderCancellationRules",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    RefundPercentage = table.Column<int>(nullable: false),
                    StaffId = table.Column<int>(nullable: false),
                    UptoHour = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderCancellationRules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProviderCancellationRules");

            migrationBuilder.DropColumn(
                name: "FollowUpDays",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "FollowUpPayRate",
                table: "Staffs");
        }
    }
}
