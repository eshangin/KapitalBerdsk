using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.BuildingObjects
{
    public class GetBuildingObjectByIdQuery : IRequest<BuildingObject>
    {
        public int Id { get; }
        public bool IncludePdSections { get; set; }
        public bool IncludePdSectionsEmployee { get; set; }
        public bool IncludeFundsFlows { get; set; }
        public bool IncludeResponsibleEmployee { get; set; }

        public GetBuildingObjectByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetBuildingObjectByIdQueryHandler : IRequestHandler<GetBuildingObjectByIdQuery, BuildingObject>
    {
        private readonly ApplicationDbContext _context;

        public GetBuildingObjectByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BuildingObject> Handle(GetBuildingObjectByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BuildingObject> query = _context.BuildingObjects;

            if (request.IncludePdSections)
            {
                query = query.Include(_ => _.PdSections);
            }
            if (request.IncludePdSectionsEmployee)
            {
                query = query.Include(_ => _.PdSections).ThenInclude(_ => _.Employee);
            }
            if (request.IncludeFundsFlows)
            {
                query = query.Include(_ => _.FundsFlows);
            }
            if (request.IncludeResponsibleEmployee)
            {
                query = query.Include(_ => _.ResponsibleEmployee);
            }

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
