using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Update_PdSection_OneTimeEmployee_Cols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdSections_Employees_EmployeeId",
                table: "PdSections");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PdSections",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "OneTimeEmployeeName",
                table: "PdSections",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PdSections_Employees_EmployeeId",
                table: "PdSections",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdSections_Employees_EmployeeId",
                table: "PdSections");

            migrationBuilder.DropColumn(
                name: "OneTimeEmployeeName",
                table: "PdSections");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "PdSections",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PdSections_Employees_EmployeeId",
                table: "PdSections",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
