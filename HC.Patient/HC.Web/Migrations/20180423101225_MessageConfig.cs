using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class MessageConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Patients_FromPatientId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Staffs_FromStaffId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_Patients_ToPatientId",
                table: "MessageRecepient");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_Staffs_ToStaffId",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_ToPatientId",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_ToStaffId",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_Message_FromPatientId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_FromStaffId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MessageRecepient");

            migrationBuilder.DropColumn(
                name: "ToPatientId",
                table: "MessageRecepient");

            migrationBuilder.DropColumn(
                name: "ToStaffId",
                table: "MessageRecepient");

            migrationBuilder.DropColumn(
                name: "FromPatientId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "FromStaffId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Message");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MessageRecepient",
                type: "BIT",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "MessageRecepient",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "GetUtcDate()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "MessageRecepient",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToUserID",
                table: "MessageRecepient",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Message",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Message",
                type: "NVARCHAR(1500)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                type: "BIT",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Message",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "GetUtcDate()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Message",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromUserID",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_ToUserID",
                table: "MessageRecepient",
                column: "ToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromUserID",
                table: "Message",
                column: "FromUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID"
               );

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_FromUserID",
                table: "Message",
                column: "FromUserID",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_ToUserID",
                table: "MessageRecepient",
                column: "ToUserID",
                principalTable: "User",
                principalColumn: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_FromUserID",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageRecepient_User_ToUserID",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_MessageRecepient_ToUserID",
                table: "MessageRecepient");

            migrationBuilder.DropIndex(
                name: "IX_Message_FromUserID",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ToUserID",
                table: "MessageRecepient");

            migrationBuilder.DropColumn(
                name: "FromUserID",
                table: "Message");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "MessageRecepient",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "MessageRecepient",
                nullable: true,
                defaultValueSql: "GetUtcDate()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "MessageRecepient",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MessageRecepient",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "ToPatientId",
                table: "MessageRecepient",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToStaffId",
                table: "MessageRecepient",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Message",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Message",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(1500)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Message",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "Message",
                nullable: true,
                defaultValueSql: "GetUtcDate()",
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedBy",
                table: "Message",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "FromPatientId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FromStaffId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Message",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_ToPatientId",
                table: "MessageRecepient",
                column: "ToPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRecepient_ToStaffId",
                table: "MessageRecepient",
                column: "ToStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromPatientId",
                table: "Message",
                column: "FromPatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromStaffId",
                table: "Message",
                column: "FromStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_CreatedBy",
                table: "Message",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Patients_FromPatientId",
                table: "Message",
                column: "FromPatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Staffs_FromStaffId",
                table: "Message",
                column: "FromStaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_User_CreatedBy",
                table: "MessageRecepient",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_Patients_ToPatientId",
                table: "MessageRecepient",
                column: "ToPatientId",
                principalTable: "Patients",
                principalColumn: "PatientID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageRecepient_Staffs_ToStaffId",
                table: "MessageRecepient",
                column: "ToStaffId",
                principalTable: "Staffs",
                principalColumn: "StaffID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
