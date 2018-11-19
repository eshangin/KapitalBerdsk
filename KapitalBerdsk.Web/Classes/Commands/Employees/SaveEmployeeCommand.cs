using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Employees
{
    public class SaveEmployeeCommand : IRequest<Employee>
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public decimal? Salary { get; set; }
        public string Email { get; set; }
        public bool IsInactive { get; set; }
    }

    public class SaveEmployeeCommandHandler : IRequestHandler<SaveEmployeeCommand, Employee>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SaveEmployeeCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Employee> Handle(SaveEmployeeCommand request, CancellationToken cancellationToken)
        {
            var item = request.Id.HasValue 
                ? await _mediator.Send(new GetEmployeeByIdQuery(request.Id.Value)) 
                : new Employee();

            item.FullName = request.FullName;
            item.Salary = request.Salary;
            item.Email = request.Email;
            item.IsInactive = request.IsInactive;

            if (!request.Id.HasValue)
            {
                _context.Employees.Add(item);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return item;
        }
    }
}
