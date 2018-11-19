using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.PdSections
{
    public class ListPdSectionsQuery : IRequest<PdSection[]>
    {
        public bool IncludeEmployee { get; set; }
        public int? BuildingObjectId { get; set; }
    }

    public class ListPdSectionsQueryHandler : IRequestHandler<ListPdSectionsQuery, PdSection[]>
    {
        private readonly ApplicationDbContext _context;

        public ListPdSectionsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PdSection[]> Handle(ListPdSectionsQuery request, CancellationToken cancellationToken)
        {
            IQueryable<PdSection> query = _context.PdSections;

            if (request.IncludeEmployee)
            {
                query = query.Include(_ => _.Employee);
            }

            if (request.BuildingObjectId.HasValue)
            {
                query = query.Where(_ => _.BuildingObjectId == request.BuildingObjectId.Value);
            }

            return await query.ApplyOrder().ToArrayAsync(cancellationToken);
        }
    }
}
