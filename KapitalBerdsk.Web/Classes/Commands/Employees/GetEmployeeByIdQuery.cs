using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Employees
{
    public class GetEmployeeByIdQuery : IRequest<Employee>
    {
        public int Id { get; }
        public bool IncludeEmployeePayrolls { get; set; }
        public bool IncludeFundsFlows { get; set; }
        public bool IncludeFundsFlowsBuildingObject { get; set; }
        public bool IncludePdSections { get; set; }
        public bool IncludePdSectionsBuildingObject { get; set; }

        public GetEmployeeByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Employee>
    {
        private readonly ApplicationDbContext _context;

        public GetEmployeeByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Employee> query = _context.Employees;

            if (request.IncludeEmployeePayrolls)
            {
                query = query.Include(_ => _.EmployeePayrolls);
            }
            if (request.IncludeFundsFlows)
            {
                query = query.Include(_ => _.FundsFlows);
            }
            if (request.IncludeFundsFlowsBuildingObject)
            {
                query = query.Include(_ => _.FundsFlows).ThenInclude(item => item.BuildingObject);
            }
            if (request.IncludePdSections)
            {
                query = query.Include(_ => _.PdSections);
            }
            if (request.IncludePdSectionsBuildingObject)
            {
                query = query.Include(_ => _.PdSections).ThenInclude(item => item.BuildingObject);
            }

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
