using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.PdSections
{
    public class GetPdSectionByIdQuery : IRequest<PdSection>
    {
        public int Id { get; }
        public bool IncludeEmployee { get; set; }

        public GetPdSectionByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetPdSectionByIdQueryHandler : IRequestHandler<GetPdSectionByIdQuery, PdSection>
    {
        private readonly ApplicationDbContext _context;

        public GetPdSectionByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PdSection> Handle(GetPdSectionByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<PdSection> query = _context.PdSections;

            if (request.IncludeEmployee)
            {
                query = query.Include(_ => _.Employee);
            }

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
