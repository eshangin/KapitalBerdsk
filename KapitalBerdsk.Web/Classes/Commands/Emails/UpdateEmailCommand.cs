using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.Emails
{
    public class UpdateEmailCommand : IRequest
    {
        public int Id { get; }
        public EmailStatus? EmailStatus { get; set; }

        public UpdateEmailCommand(int id)
        {
            Id = id;
        }
    }

    public class UpdateEmailCommandHandler : AsyncRequestHandler<UpdateEmailCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UpdateEmailCommandHandler(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        protected override async Task Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            Email email = await _mediator.Send(new GetEmailByIdQuery(request.Id));

            if (email != null)
            {
                if (request.EmailStatus.HasValue)
                {
                    email.Status = request.EmailStatus.Value;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
