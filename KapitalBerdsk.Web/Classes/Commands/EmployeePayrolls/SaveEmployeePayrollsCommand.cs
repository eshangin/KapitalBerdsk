using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.EmployeePayrolls
{
    public class SaveEmployeePayrollsCommand : IRequest
    {
        public EmployeePayroll[] Payrolls { get; }

        public SaveEmployeePayrollsCommand(EmployeePayroll[] _payrolls)
        {
            Payrolls = _payrolls;
        }
    }

    public class SaveEmployeePayrollsCommandHandler : AsyncRequestHandler<SaveEmployeePayrollsCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SaveEmployeePayrollsCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        protected override async Task Handle(SaveEmployeePayrollsCommand request, CancellationToken cancellationToken)
        {
            _context.EmployeePayrolls.AddRange(request.Payrolls);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
