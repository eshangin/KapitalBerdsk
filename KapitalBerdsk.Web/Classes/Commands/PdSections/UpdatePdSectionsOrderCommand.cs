using KapitalBerdsk.Web.Classes.Commands.Common;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Commands.PdSections
{
    public class UpdatePdSectionsOrderCommand : UpdateOrderCommand
    {
        public UpdatePdSectionsOrderCommand(IEnumerable<int> idsWithNewOrder)
            : base(idsWithNewOrder)
        {
            
        }
    }

    public class UpdatePdSectionsOrderCommandHandler : UpdateOrderCommandHandler<UpdatePdSectionsOrderCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdatePdSectionsOrderCommandHandler(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        protected override async Task<IEnumerable<IOrderable>> GetOrderables()
        {
            return await _context.PdSections.ToListAsync();
        }
    }
}
