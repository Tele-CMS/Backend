using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MasterServiceCodeModifiersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterServiceCodeModifiers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Modifier = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Rate = table.Column<decimal>(nullable: false),
                    ServiceCode = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ServiceCodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterServiceCodeModifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MasterServiceCodeModifiers_MasterServiceCode_ServiceCodeId",
                        column: x => x.ServiceCodeId,
                        principalTable: "MasterServiceCode",
                        principalColumn: "ServiceCodeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterServiceCodeModifiers_ServiceCodeId",
                table: "MasterServiceCodeModifiers",
                column: "ServiceCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterServiceCodeModifiers");
        }
    }
}
