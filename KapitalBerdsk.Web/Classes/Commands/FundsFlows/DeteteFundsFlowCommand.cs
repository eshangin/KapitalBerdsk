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
    public class DeteteFundsFlowCommand : IRequest
    {
        public int Id { get; }

        public DeteteFundsFlowCommand(int id)
        {
            Id = id;
        }
    }

    public class DeteteFundsFlowCommandHandler : AsyncRequestHandler<DeteteFundsFlowCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeteteFundsFlowCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(DeteteFundsFlowCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = new FundsFlow() { Id = request.Id };
            _context.Entry(itemToDelete).State = EntityState.Deleted;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
