using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Create_FundsFlow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundsFlows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BuildingObjectId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    Income = table.Column<decimal>(nullable: true),
                    Outgo = table.Column<decimal>(nullable: true),
                    PayType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundsFlows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundsFlows_BuildingObjects_BuildingObjectId",
                        column: x => x.BuildingObjectId,
                        principalTable: "BuildingObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FundsFlows_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundsFlows_BuildingObjectId",
                table: "FundsFlows",
                column: "BuildingObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_FundsFlows_EmployeeId",
                table: "FundsFlows",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundsFlows");
        }
    }
}
