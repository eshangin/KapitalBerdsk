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
    public class GetFundsFlowByIdQuery : IRequest<FundsFlow>
    {
        public int Id { get; }
        public bool IncludeEmployee { get; set; }
        public bool IncludeBuildingObject { get; set; }
        public bool IncludeOrganization { get; set; }

        public GetFundsFlowByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetFundsFlowByIdQueryHandler : IRequestHandler<GetFundsFlowByIdQuery, FundsFlow>
    {
        private readonly ApplicationDbContext _context;

        public GetFundsFlowByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FundsFlow> Handle(GetFundsFlowByIdQuery request, CancellationToken cancellationToken)
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

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
