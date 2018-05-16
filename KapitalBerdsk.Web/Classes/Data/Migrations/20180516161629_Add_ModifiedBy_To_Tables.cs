using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Add_ModifiedBy_To_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "PdSections",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "FundsFlows",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "BuildingObjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PdSections_ModifiedById",
                table: "PdSections",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_ModifiedById",
                table: "Organizations",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_FundsFlows_ModifiedById",
                table: "FundsFlows",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ModifiedById",
                table: "Employees",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_BuildingObjects_ModifiedById",
                table: "BuildingObjects",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ModifiedById",
                table: "AspNetUsers",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ModifiedById",
                table: "AspNetUsers",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BuildingObjects_AspNetUsers_ModifiedById",
                table: "BuildingObjects",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ModifiedById",
                table: "Employees",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FundsFlows_AspNetUsers_ModifiedById",
                table: "FundsFlows",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_AspNetUsers_ModifiedById",
                table: "Organizations",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PdSections_AspNetUsers_ModifiedById",
                table: "PdSections",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ModifiedById",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BuildingObjects_AspNetUsers_ModifiedById",
                table: "BuildingObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ModifiedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_FundsFlows_AspNetUsers_ModifiedById",
                table: "FundsFlows");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_AspNetUsers_ModifiedById",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_PdSections_AspNetUsers_ModifiedById",
                table: "PdSections");

            migrationBuilder.DropIndex(
                name: "IX_PdSections_ModifiedById",
                table: "PdSections");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_ModifiedById",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_FundsFlows_ModifiedById",
                table: "FundsFlows");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ModifiedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_BuildingObjects_ModifiedById",
                table: "BuildingObjects");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ModifiedById",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PdSections");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "FundsFlows");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "AspNetUsers");
        }
    }
}
