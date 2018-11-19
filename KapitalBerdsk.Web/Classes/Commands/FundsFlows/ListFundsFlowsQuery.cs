using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.FundsFlows
{
    public class ListFundsFlowsQuery : IRequest<FundsFlow[]>
    {
        public string UserId { get; set; }
        public bool IncludeEmployee { get; set; }
        public bool IncludeBuildingObject { get; set; }
        public bool IncludeOrganization { get; set; }
    }

    public class ListFundsFlowsQueryHandler : IRequestHandler<ListFundsFlowsQuery, FundsFlow[]>
    {
        private readonly ApplicationDbContext _context;

        public ListFundsFlowsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FundsFlow[]> Handle(ListFundsFlowsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<FundsFlow> query = _context.FundsFlows;

            if (request.IncludeBuildingObject)
            {
                query = query.Include(_ => _.BuildingObject);
            }
            if (request.IncludeEmployee)
            {
                query = query.Include(_ => _.Employee);
            }
            if (request.IncludeOrganization)
            {
                query = query.Include(_ => _.Organization);
            }

            if (!string.IsNullOrEmpty(request.UserId))
            {
                query = query.Where(ff => ff.CreatedById == request.UserId);
            }

            query = query.OrderByDescending(_ => _.Date).ThenByDescending(_ => _.Id);

            return await query.ToArrayAsync(cancellationToken);
        }
    }
}
