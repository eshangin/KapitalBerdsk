using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.BuildingObjects
{
    public class ListBuildingObjectsQuery : IRequest<BuildingObject[]>
    {
        public bool IncludePdSections { get; set; }
        public bool IncludePdSectionsEmployee { get; set; }
        public bool IncludeFundsFlows { get; set; }
        public bool IncludeResponsibleEmployee { get; set; }
        public bool OnlyActive { get; set; }
        public DateTime? MaxContractDateEnd { get; set; }
        public BuildingObjectStatus? WithStatus { get; set; }
    }

    public class ListBuildingObjectsQueryHandler : IRequestHandler<ListBuildingObjectsQuery, BuildingObject[]>
    {
        private readonly ApplicationDbContext _context;

        public ListBuildingObjectsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BuildingObject[]> Handle(ListBuildingObjectsQuery request, CancellationToken cancellationToken)
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

            if (request.OnlyActive)
            {
                query = query.OnlyActive();
            }
            if (request.MaxContractDateEnd.HasValue)
            {
                query = query.Where(_ => _.ContractDateEnd <= request.MaxContractDateEnd.Value);
            }
            if (request.WithStatus.HasValue)
            {
                query = query.Where(_ => _.Status == request.WithStatus.Value);
            }

            return await query.ApplyOrder().ToArrayAsync(cancellationToken);
        }
    }
}
