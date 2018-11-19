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
    public class ListOneTimeEmployeesQuery : IRequest<string[]>
    {
    }

    public class ListOneTimeEmployeesQueryHandler : IRequestHandler<ListOneTimeEmployeesQuery, string[]>
    {
        private readonly ApplicationDbContext _context;

        public ListOneTimeEmployeesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string[]> Handle(ListOneTimeEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await _context.FundsFlows
                .Where(ff => !string.IsNullOrWhiteSpace(ff.OneTimeEmployeeName))
                .Select(ff => ff.OneTimeEmployeeName)
                .ToArrayAsync(cancellationToken);
        }
    }
}
