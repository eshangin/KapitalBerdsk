using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class AddColumn_BuildingObjects_ResponsibleEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResponsibleEmployeeId",
                table: "BuildingObjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BuildingObjects_ResponsibleEmployeeId",
                table: "BuildingObjects",
                column: "ResponsibleEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingObjects_Employees_ResponsibleEmployeeId",
                table: "BuildingObjects",
                column: "ResponsibleEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuildingObjects_Employees_ResponsibleEmployeeId",
                table: "BuildingObjects");

            migrationBuilder.DropIndex(
                name: "IX_BuildingObjects_ResponsibleEmployeeId",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "ResponsibleEmployeeId",
                table: "BuildingObjects");
        }
    }
}
