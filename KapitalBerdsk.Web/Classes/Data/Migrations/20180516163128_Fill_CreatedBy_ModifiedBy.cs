using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KapitalBerdsk.Web.Classes.Data.Migrations
{
    public partial class Fill_CreatedBy_ModifiedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
declare @admin nvarchar(450)

select top (1) @admin = u.Id from 
dbo.AspNetUsers u 
order by u.datecreated

update dbo.AspNetUsers set CreatedById = @admin, ModifiedById = @admin

declare @firstManager nvarchar(450)

select top (1) @firstManager = u.Id from 
dbo.AspNetUsers u 
inner join dbo.AspNetUserRoles ur on u.Id = ur.UserId
inner join dbo.AspNetRoles r on ur.RoleId = r.Id
where r.Name = 'Manager'
order by u.datecreated

update dbo.BuildingObjects set CreatedById = @firstManager, ModifiedById = @firstManager
update dbo.Employees set CreatedById = @firstManager, ModifiedById = @firstManager
update dbo.FundsFlows set CreatedById = @firstManager, ModifiedById = @firstManager
update dbo.Organizations set CreatedById = @firstManager, ModifiedById = @firstManager
update dbo.PdSections set CreatedById = @firstManager, ModifiedById = @firstManager
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
