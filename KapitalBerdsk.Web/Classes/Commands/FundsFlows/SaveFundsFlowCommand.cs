using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Data.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.FundsFlows
{
    public class SaveFundsFlowCommand : IRequest<FundsFlow>
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public int? BuildingObjectId { get; set; }
        public string Description { get; set; }
        public int? OrganizationId { get; set; }
        public decimal? Income { get; set; }
        public decimal? Outgo { get; set; }
        public OutgoType OutgoType { get; set; }
        public PaymentType PayType { get; set; }
        public bool UseOneTimeEmployee { get; set; }
        public int? EmployeeId { get; set; }
        public string OneTimeEmployeeName { get; set; }
    }

    public class SaveFundsFlowCommandHandler : IRequestHandler<SaveFundsFlowCommand, FundsFlow>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SaveFundsFlowCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<FundsFlow> Handle(SaveFundsFlowCommand request, CancellationToken cancellationToken)
        {
            var ff = request.Id.HasValue
                ? await _mediator.Send(new GetFundsFlowByIdQuery(request.Id.Value))
                : new FundsFlow();
            ff.Date = request.Date;
            ff.BuildingObjectId = request.BuildingObjectId;
            ff.Description = request.Description;
            ff.OrganizationId = request.OrganizationId;
            ff.Income = request.Income;
            ff.Outgo = request.Outgo;
            ff.OutgoType = request.OutgoType;
            ff.PayType = request.PayType;
            ff.SetEmployee(request.UseOneTimeEmployee, request.EmployeeId, request.OneTimeEmployeeName);

            if (!request.Id.HasValue)
            {
                _context.FundsFlows.Add(ff);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return ff;
        }
    }
}
