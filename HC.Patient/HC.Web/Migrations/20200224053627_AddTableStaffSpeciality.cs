using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddTableStaffSpeciality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSpecialities_User_CreatedBy",
                table: "UserSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpecialities_User_DeletedBy",
                table: "UserSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpecialities_Staffs_StaffsId",
                table: "UserSpecialities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpecialities_User_UpdatedBy",
                table: "UserSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSpecialities",
                table: "UserSpecialities");

            migrationBuilder.RenameTable(
                name: "UserSpecialities",
                newName: "UserSpeciality");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpecialities_UpdatedBy",
                table: "UserSpeciality",
                newName: "IX_UserSpeciality_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpecialities_StaffsId",
                table: "UserSpeciality",
                newName: "IX_UserSpeciality_StaffsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpecialities_DeletedBy",
                table: "UserSpeciality",
                newName: "IX_UserSpeciality_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpecialities_CreatedBy",
                table: "UserSpeciality",
                newName: "IX_UserSpeciality_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSpeciality",
                table: "UserSpeciality",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StaffSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<int>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<int>(nullable: true),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    GlobalCodeId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StaffID = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffSpecialities_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffSpecialities_User_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StaffSpecialities_GlobalCode_GlobalCodeId",
                        column: x => x.GlobalCodeId,
                        principalTable: "GlobalCode",
                        principalColumn: "GlobalCodeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffSpecialities_Staffs_StaffID",
                        column: x => x.StaffID,
                        principalTable: "Staffs",
                        principalColumn: "StaffID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffSpecialities_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffSpecialities_CreatedBy",
                table: "StaffSpecialities",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSpecialities_DeletedBy",
                table: "StaffSpecialities",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSpecialities_GlobalCodeId",
                table: "StaffSpecialities",
                column: "GlobalCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSpecialities_StaffID",
                table: "StaffSpecialities",
                column: "StaffID");

            migrationBuilder.CreateIndex(
                name: "IX_StaffSpecialities_UpdatedBy",
                table: "StaffSpecialities",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpeciality_User_CreatedBy",
                table: "UserSpeciality",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpeciality_User_DeletedBy",
                table: "UserSpeciality",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpeciality_Staffs_StaffsId",
                table: "UserSpeciality",
                column: "StaffsId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpeciality_User_UpdatedBy",
                table: "UserSpeciality",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSpeciality_User_CreatedBy",
                table: "UserSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpeciality_User_DeletedBy",
                table: "UserSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpeciality_Staffs_StaffsId",
                table: "UserSpeciality");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSpeciality_User_UpdatedBy",
                table: "UserSpeciality");

            migrationBuilder.DropTable(
                name: "StaffSpecialities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSpeciality",
                table: "UserSpeciality");

            migrationBuilder.RenameTable(
                name: "UserSpeciality",
                newName: "UserSpecialities");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpeciality_UpdatedBy",
                table: "UserSpecialities",
                newName: "IX_UserSpecialities_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpeciality_StaffsId",
                table: "UserSpecialities",
                newName: "IX_UserSpecialities_StaffsId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpeciality_DeletedBy",
                table: "UserSpecialities",
                newName: "IX_UserSpecialities_DeletedBy");

            migrationBuilder.RenameIndex(
                name: "IX_UserSpeciality_CreatedBy",
                table: "UserSpecialities",
                newName: "IX_UserSpecialities_CreatedBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSpecialities",
                table: "UserSpecialities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpecialities_User_CreatedBy",
                table: "UserSpecialities",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpecialities_User_DeletedBy",
                table: "UserSpecialities",
                column: "DeletedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpecialities_Staffs_StaffsId",
                table: "UserSpecialities",
                column: "StaffsId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSpecialities_User_UpdatedBy",
                table: "UserSpecialities",
                column: "UpdatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
