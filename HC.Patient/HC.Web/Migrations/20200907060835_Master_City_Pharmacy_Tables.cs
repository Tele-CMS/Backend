using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class Master_City_Pharmacy_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterCity",
                columns: table => new
                {
                    CityID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityName = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StateID = table.Column<int>(nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCity", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_MasterCity_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterCity_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterCity_MasterState_StateID",
                        column: x => x.StateID,
                        principalTable: "MasterState",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterCity_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MasterPharmacy",
                columns: table => new
                {
                    PharmacyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CityID = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PharmacyAddress = table.Column<string>(nullable: true),
                    PharmacyFaxNumber = table.Column<string>(nullable: true),
                    PharmacyName = table.Column<string>(maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterPharmacy", x => x.PharmacyID);
                    table.ForeignKey(
                        name: "FK_MasterPharmacy_MasterCity_CityID",
                        column: x => x.CityID,
                        principalTable: "MasterCity",
                        principalColumn: "CityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterPharmacy_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterPharmacy_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MasterPharmacy_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterCity_CreatedBy",
                table: "MasterCity",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCity_DeletedBy",
                table: "MasterCity",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCity_StateID",
                table: "MasterCity",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterCity_UpdatedBy",
                table: "MasterCity",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPharmacy_CityID",
                table: "MasterPharmacy",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPharmacy_CreatedBy",
                table: "MasterPharmacy",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPharmacy_DeletedBy",
                table: "MasterPharmacy",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MasterPharmacy_UpdatedBy",
                table: "MasterPharmacy",
                column: "UpdatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterPharmacy");

            migrationBuilder.DropTable(
                name: "MasterCity");
        }
    }
}
