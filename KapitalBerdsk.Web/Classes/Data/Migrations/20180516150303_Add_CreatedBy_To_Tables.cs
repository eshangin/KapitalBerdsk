using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Add_CreatedBy_To_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "PdSections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "FundsFlows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "BuildingObjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PdSections_CreatedById",
                table: "PdSections",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_CreatedById",
                table: "Organizations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_FundsFlows_CreatedById",
                table: "FundsFlows",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingObjects_CreatedById",
                table: "BuildingObjects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingObjects_AspNetUsers_CreatedById",
                table: "BuildingObjects",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedById",
                table: "Employees",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FundsFlows_AspNetUsers_CreatedById",
                table: "FundsFlows",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AspNetUsers_CreatedById",
                table: "Organizations",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PdSections_AspNetUsers_CreatedById",
                table: "PdSections",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BuildingObjects_AspNetUsers_CreatedById",
                table: "BuildingObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_CreatedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_FundsFlows_AspNetUsers_CreatedById",
                table: "FundsFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AspNetUsers_CreatedById",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PdSections_AspNetUsers_CreatedById",
                table: "PdSections");

            migrationBuilder.DropIndex(
                name: "IX_PdSections_CreatedById",
                table: "PdSections");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_CreatedById",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_FundsFlows_CreatedById",
                table: "FundsFlows");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_BuildingObjects_CreatedById",
                table: "BuildingObjects");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "PdSections");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "FundsFlows");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AspNetUsers");
        }
    }
}
