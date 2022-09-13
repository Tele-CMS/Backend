﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace HC.Patient.Web.Migrations
{
    public partial class AddedNewColumnsIsProviderEducationalDocumentToTableUserDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProviderEducationalDocument",
                table: "UserDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProviderEducationalDocument",
                table: "UserDocuments");
        }
    }
}
