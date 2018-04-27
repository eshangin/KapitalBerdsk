using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Data.Migrations
{
    public partial class Empl_BuildingObj_UniqueKyes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employees_FullName",
                table: "Employees",
                column: "FullName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingObjects_Name",
                table: "BuildingObjects",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_FullName",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_BuildingObjects_Name",
                table: "BuildingObjects");
        }
    }
}
