using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedPayerServiceModifiersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayerServiceCodeModifiers",
                columns: table => new
                {
                    ModifierId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Modifier = table.Column<string>(type: "varchar(20)", nullable: true),
                    PayerServiceCodeId = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    ServiceCode = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayerServiceCodeModifiers", x => x.ModifierId);
                    table.ForeignKey(
                        name: "FK_PayerServiceCodeModifiers_PayerServiceCodes_PayerServiceCodeId",
                        column: x => x.PayerServiceCodeId,
                        principalTable: "PayerServiceCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayerServiceCodeModifiers_PayerServiceCodeId",
                table: "PayerServiceCodeModifiers",
                column: "PayerServiceCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayerServiceCodeModifiers");
        }
    }
}
