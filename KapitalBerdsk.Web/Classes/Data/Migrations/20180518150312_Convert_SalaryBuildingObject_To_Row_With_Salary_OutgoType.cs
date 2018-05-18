using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Convert_SalaryBuildingObject_To_Row_With_Salary_OutgoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
with ff (Id, BuildingObjectId, OutgoType)
AS
(
	select ff.Id, ff.BuildingObjectId, ff.OutgoType
	from 
		dbo.FundsFlows ff
		inner join dbo.BuildingObjects bo on ff.BuildingObjectId = bo.Id
	where
		bo.Name = N'оклад'
)
update ff set BuildingObjectId = NULL, OutgoType = 4

delete from dbo.BuildingObjects where Name = N'оклад'
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
