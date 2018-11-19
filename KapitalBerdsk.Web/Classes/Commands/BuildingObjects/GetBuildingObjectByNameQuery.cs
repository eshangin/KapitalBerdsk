using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.BuildingObjects
{
    public class GetBuildingObjectByNameQuery : IRequest<BuildingObject>
    {
        public string Name { get; }

        public GetBuildingObjectByNameQuery(string name)
        {
            Name = name;
        }
    }

    public class GetBuildingObjectByNameQueryHandler : IRequestHandler<GetBuildingObjectByNameQuery, BuildingObject>
    {
        private readonly ApplicationDbContext _context;

        public GetBuildingObjectByNameQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BuildingObject> Handle(GetBuildingObjectByNameQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BuildingObject> query = _context.BuildingObjects;

            return await query
                .Where(item => item.Name == request.Name)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
