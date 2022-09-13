using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnPhotoPathPhotoThumbNailPathGlobalCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "GlobalCode",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoThumbnailPath",
                table: "GlobalCode",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "GlobalCode");

            migrationBuilder.DropColumn(
                name: "PhotoThumbnailPath",
                table: "GlobalCode");
        }
    }
}
