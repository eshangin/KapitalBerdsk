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

namespace KapitalBerdsk.Web.Classes.Commands.Employees
{
    public class UpdateEmployeesOrderCommand : UpdateOrderCommand
    {
        public UpdateEmployeesOrderCommand(IEnumerable<int> idsWithNewOrder)
            : base(idsWithNewOrder)
        {
            
        }
    }

    public class UpdateEmployeesOrderCommandHandler : UpdateOrderCommandHandler<UpdateEmployeesOrderCommand>
    {
        private readonly ApplicationDbContext _context;

        public UpdateEmployeesOrderCommandHandler(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        protected override async Task<IEnumerable<IOrderable>> GetOrderables()
        {
            return await _context.Employees.ToListAsync();
        }
    }
}
