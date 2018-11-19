using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Employees
{
    public class ListEmployeesQuery : IRequest<Employee[]>
    {
        public bool OnlyActive { get; set; }
        public bool IncludeEmployeePayrolls { get; set; }
        public bool IncludePdSections { get; set; }
        public bool IncludeFundsFlows { get; set; }
        public bool WithSalaryOnly { get; set; }
    }

    public class ListEmployeesQueryHandler : IRequestHandler<ListEmployeesQuery, Employee[]>
    {
        private readonly ApplicationDbContext _context;

        public ListEmployeesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee[]> Handle(ListEmployeesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Employee> query = _context.Employees;

            if (request.IncludeEmployeePayrolls)
            {
                query = query.Include(_ => _.EmployeePayrolls);
            }
            if (request.IncludePdSections)
            {
                query = query.Include(_ => _.PdSections);
            }
            if (request.IncludeFundsFlows)
            {
                query = query.Include(_ => _.FundsFlows);
            }

            if (request.OnlyActive)
            {
                query = query.OnlyActive();
            }
            if (request.WithSalaryOnly)
            {
                query = query.Where(e => e.Salary.HasValue && e.Salary.Value > 0);
            }

            return await query
                .OrderBy(item => item.OrderNumber)
                .ThenByDescending(item => item.Id)
                .ToArrayAsync(cancellationToken);
        }
    }
}
