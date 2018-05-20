using KapitalBerdsk.Web.Classes.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public class PayEmployeePayrollService : IPayEmployeePayrollService
    {
        private readonly ApplicationDbContext _context;

        public PayEmployeePayrollService(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task PayToAllEmployees()
        {
            DateTime today = DateTime.UtcNow.AddHours(7).Date;
            DateTime prevMonth = today.AddMonths(-1);
            List<Employee> employees = await _context.Employees.Where(e => e.Salary.HasValue &&
                                                                                e.Salary.Value > 0).ToListAsync();
            foreach (var emp in employees)
            {
                await _context.EmployeePayrolls.AddAsync(new EmployeePayroll()
                {
                    EmployeeId = emp.Id,
                    Value = emp.Salary.Value,
                    Year = prevMonth.Year,
                    Month = prevMonth.Month
                });
            }
            await _context.SaveChangesAsync();
        }
    }
}
