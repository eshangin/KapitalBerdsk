using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class FundsFlow_EmployeeId_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundsFlows_Employees_EmployeeId",
                table: "FundsFlows");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "FundsFlows",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_FundsFlows_Employees_EmployeeId",
                table: "FundsFlows",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FundsFlows_Employees_EmployeeId",
                table: "FundsFlows");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "FundsFlows",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FundsFlows_Employees_EmployeeId",
                table: "FundsFlows",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
