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
    public class DetetePdSectionCommand : IRequest
    {
        public int Id { get; }

        public DetetePdSectionCommand(int id)
        {
            Id = id;
        }
    }

    public class DetetePdSectionCommandHandler : AsyncRequestHandler<DetetePdSectionCommand>
    {
        private readonly ApplicationDbContext _context;

        public DetetePdSectionCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(DetetePdSectionCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = new PdSection() { Id = request.Id };
            _context.Entry(itemToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
