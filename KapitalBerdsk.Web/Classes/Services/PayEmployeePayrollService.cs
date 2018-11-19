using KapitalBerdsk.Web.Classes.Commands.EmployeePayrolls;
using KapitalBerdsk.Web.Classes.Commands.Employees;
using KapitalBerdsk.Web.Classes.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Classes.Services
{
    public class PayEmployeePayrollService : IPayEmployeePayrollService
    {
        private readonly IMediator _mediator;
        private readonly IDateTimeService _dateTimeService;

        public PayEmployeePayrollService(
            IDateTimeService dateTimeService,
            IMediator mediator)
        {
            _dateTimeService = dateTimeService;
            _mediator = mediator;
        }

        public async Task PayToAllEmployees()
        {
            DateTime today = _dateTimeService.LocalDate;
            DateTime prevMonth = today.AddMonths(-1);
            Employee[] employees = await _mediator.Send(new ListEmployeesQuery()
            {
                WithSalaryOnly = true
            });

            await _mediator.Send(new SaveEmployeePayrollsCommand(employees.Select(emp => new EmployeePayroll()
            {
                EmployeeId = emp.Id,
                Value = emp.Salary.Value,
                Year = prevMonth.Year,
                Month = prevMonth.Month
            }).ToArray()));
        }
    }
}
