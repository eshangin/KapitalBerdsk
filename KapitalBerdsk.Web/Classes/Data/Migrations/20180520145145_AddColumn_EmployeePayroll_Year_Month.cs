using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class AddColumn_EmployeePayroll_Year_Month : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "EmployeePayrolls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "EmployeePayrolls",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "EmployeePayrolls");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "EmployeePayrolls");
        }
    }
}
