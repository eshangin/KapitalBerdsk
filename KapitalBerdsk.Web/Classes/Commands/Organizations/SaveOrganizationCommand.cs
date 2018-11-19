using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Organizations
{
    public class SaveOrganizationCommand : IRequest<Organization>
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsInactive { get; set; }
    }

    public class SaveOrganizationCommandHandler : IRequestHandler<SaveOrganizationCommand, Organization>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SaveOrganizationCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Organization> Handle(SaveOrganizationCommand request, CancellationToken cancellationToken)
        {
            var org = request.Id.HasValue 
                ? await _mediator.Send(new GetOrganizationByIdQuery(request.Id.Value)) 
                : new Organization();

            org.Name = request.Name;
            org.IsInactive = request.IsInactive;

            if (!request.Id.HasValue)
            {
                _context.Organizations.Add(org);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return org;
        }
    }
}
