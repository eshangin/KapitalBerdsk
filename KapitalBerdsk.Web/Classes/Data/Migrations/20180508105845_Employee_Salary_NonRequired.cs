﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Employee_Salary_NonRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
