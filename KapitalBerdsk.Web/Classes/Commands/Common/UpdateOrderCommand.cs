using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Common
{
    public abstract class UpdateOrderCommand : IRequest
    {
        public IEnumerable<int> IdsWithNewOrder { get; }

        public UpdateOrderCommand(IEnumerable<int> idsWithNewOrder)
        {
            IdsWithNewOrder = idsWithNewOrder;
        }
    }

    public abstract class UpdateOrderCommandHandler<TCommand> : AsyncRequestHandler<TCommand>
        where TCommand : UpdateOrderCommand, IRequest
    {
        private readonly ApplicationDbContext _context;

        public UpdateOrderCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected abstract Task<IEnumerable<IOrderable>> GetOrderables();

        protected override async Task Handle(TCommand request, CancellationToken cancellationToken)
        {
            (await GetOrderables()).UpdateOrder(request.IdsWithNewOrder);
            await _context.SaveChangesAsync();
        }
    }
}
