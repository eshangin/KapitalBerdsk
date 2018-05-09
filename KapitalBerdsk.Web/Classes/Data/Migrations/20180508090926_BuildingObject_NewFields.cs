using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class BuildingObject_NewFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ContractDateEnd",
                table: "BuildingObjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ContractDateStart",
                table: "BuildingObjects",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "BuildingObjects",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BuildingObjects",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContractDateEnd",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "ContractDateStart",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "BuildingObjects");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BuildingObjects");
        }
    }
}
