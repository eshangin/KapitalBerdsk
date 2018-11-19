using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.PdSections
{
    public class SavePdSectionCommand : IRequest<PdSection>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int BuildingObjectId {get;set;}
        public decimal Price { get; set; }
        public bool UseOneTimeEmployee;
        public int? EmployeeId { get; set; }
        public string OneTimeEmployeeName { get; set; }
    }

    public class SavePdSectionCommandHandler : IRequestHandler<SavePdSectionCommand, PdSection>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SavePdSectionCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<PdSection> Handle(SavePdSectionCommand request, CancellationToken cancellationToken)
        {
            var item = request.Id.HasValue 
                ? await _mediator.Send(new GetPdSectionByIdQuery(request.Id.Value)) 
                : new PdSection();

            item.Name = request.Name;
            item.BuildingObjectId = request.BuildingObjectId;
            item.Price = request.Price;
            item.SetEmployee(request.UseOneTimeEmployee, request.EmployeeId, request.OneTimeEmployeeName);

            if (!request.Id.HasValue)
            {
                _context.PdSections.Add(item);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return item;
        }
    }
}
