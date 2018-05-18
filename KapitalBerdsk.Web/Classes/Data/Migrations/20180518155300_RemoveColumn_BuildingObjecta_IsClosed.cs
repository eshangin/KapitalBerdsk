using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class RemoveColumn_BuildingObjecta_IsClosed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
update dbo.BuildingObjects set Status = 3 where IsClosed = 1
");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "BuildingObjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "BuildingObjects",
                nullable: false,
                defaultValue: false);
        }
    }
}
