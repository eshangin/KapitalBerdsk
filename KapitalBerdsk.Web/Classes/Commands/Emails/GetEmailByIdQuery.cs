using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Emails
{
    public class GetEmailByIdQuery : IRequest<Email>
    {
        public int Id { get; }

        public GetEmailByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetEmailByIdQueryHandler : IRequestHandler<GetEmailByIdQuery, Email>
    {
        private readonly ApplicationDbContext _context;

        public GetEmailByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Email> Handle(GetEmailByIdQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Email> query = _context.Emails;

            return await query
                .Where(item => item.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
