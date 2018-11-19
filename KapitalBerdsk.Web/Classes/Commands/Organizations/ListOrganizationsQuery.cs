using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Organizations
{
    public class ListOrganizationsQuery : IRequest<Organization[]>
    {
        public bool IncludeFundsFlows { get; set; }
        public bool OnlyActive { get; set; }
    }

    public class ListOrganizationsQueryHandler : IRequestHandler<ListOrganizationsQuery, Organization[]>
    {
        private readonly ApplicationDbContext _context;

        public ListOrganizationsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Organization[]> Handle(ListOrganizationsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Organization> query = _context.Organizations;

            if (request.IncludeFundsFlows)
            {
                query = query.Include(_ => _.FundsFlows);
            }
            if (request.OnlyActive)
            {
                query = query.OnlyActive();
            }

            return await query.ToArrayAsync(cancellationToken);
        }
    }
}
