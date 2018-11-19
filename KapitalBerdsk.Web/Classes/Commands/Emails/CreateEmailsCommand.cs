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
    public class CreateEmailsCommand : IRequest
    {
        public IEnumerable<Email> Emails { get; }

        public CreateEmailsCommand(IEnumerable<Email> emails)
        {
            Emails = emails;
        }
    }

    public class CreateEmailsCommandHandler : AsyncRequestHandler<CreateEmailsCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateEmailsCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task Handle(CreateEmailsCommand request, CancellationToken cancellationToken)
        {
            _context.Emails.AddRange(request.Emails);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
