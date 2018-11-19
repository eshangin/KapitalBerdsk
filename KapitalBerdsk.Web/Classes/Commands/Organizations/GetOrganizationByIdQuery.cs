using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Organizations
{
    public class GetOrganizationByIdQuery : IRequest<Organization>
    {
        public int Id { get; }
        public bool IncludeFundsFlows { get; set; }

        public GetOrganizationByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Organization>
    {
        private readonly ApplicationDbContext _context;

        public GetOrganizationByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Organization> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Organization> query = _context.Organizations;

            if (request.IncludeFundsFlows)
            {
                query = query.Include(_ => _.FundsFlows);
            }

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
