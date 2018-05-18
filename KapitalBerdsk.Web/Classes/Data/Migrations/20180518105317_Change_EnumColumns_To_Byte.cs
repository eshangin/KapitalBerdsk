using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Change_EnumColumns_To_Byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PayType",
                table: "FundsFlows",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<byte>(
                name: "OutgoType",
                table: "FundsFlows",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(int),
                oldDefaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PayType",
                table: "FundsFlows",
                nullable: false,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<int>(
                name: "OutgoType",
                table: "FundsFlows",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(byte),
                oldDefaultValue: (byte)1);
        }
    }
}
