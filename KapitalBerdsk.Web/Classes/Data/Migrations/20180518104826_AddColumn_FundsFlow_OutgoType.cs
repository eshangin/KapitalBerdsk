using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class AddColumn_FundsFlow_OutgoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OutgoType",
                table: "FundsFlows",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutgoType",
                table: "FundsFlows");
        }
    }
}
