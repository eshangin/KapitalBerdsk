using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Employees
{
    public class GetEmployeeByNameQuery : IRequest<Employee>
    {
        public string Name { get; }

        public GetEmployeeByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetEmployeeByNameQueryHandler : IRequestHandler<GetEmployeeByNameQuery, Employee>
    {
        private readonly ApplicationDbContext _context;

        public GetEmployeeByNameQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> Handle(GetEmployeeByNameQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Employee> query = _context.Employees;

            return await query
                .Where(item => item.FullName == request.Name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
