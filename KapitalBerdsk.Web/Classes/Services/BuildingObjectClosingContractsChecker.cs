using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Commands.BuildingObjects;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Services
{
    public class BuildingObjectClosingContractsChecker : IBuildingObjectClosingContractsChecker
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediator;

        public BuildingObjectClosingContractsChecker(
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            IDateTimeService dateTimeService,
            IMediator mediator)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _dateTimeService = dateTimeService;
            _mediator = mediator;
        }

        public async Task Check()
        {
            IEnumerable<BuildingObject> items = await GetBuildingObjectWithClosingContracts();

            if (items.Any())
            {
                const string messageSubject = "Окончание контрактов";
                IList<ApplicationUser> managers = await _userManager.GetUsersInRoleAsync(Constants.Roles.Manager);
                IEnumerable<Employee> responsibleEmployees = from item in items
                                                             where item.ResponsibleEmployee != null &&
                                                                !string.IsNullOrWhiteSpace(item.ResponsibleEmployee.Email)
                                                             select item.ResponsibleEmployee;

                var emails = new List<Email>();

                string messageForManagers = BuildMessageBody(items);
                foreach (ApplicationUser m in managers)
                {
                    emails.Add(new Email
                    {
                        ToCsv = m.Email,
                        Subject = messageSubject,
                        Body = messageForManagers
                    });
                }

                foreach (Employee emp in responsibleEmployees)
                {
                    if (!managers.Select(m => m.Email).Contains(emp.Email))
                    {
                        IEnumerable<BuildingObject> responsibleFor = items.Where(item => item.ResponsibleEmployeeId == emp.Id);
                        string messageForResponsible = BuildMessageBody(responsibleFor);
                        emails.Add(new Email
                        {
                            ToCsv = emp.Email,
                            Subject = messageSubject,
                            Body = messageForResponsible
                        });
                    }
                }

                await _emailSender.AddPendingEmails(emails);
            }
        }

        private string BuildMessageBody(IEnumerable<BuildingObject> buildingObjectsWithClosingContracts)
        {
            DateTime today = _dateTimeService.LocalDate;

            string message = "<h3>Приближается срок окончания контрактов:</h3><ul>";
            foreach (BuildingObject item in buildingObjectsWithClosingContracts)
            {
                int dayBefore = (item.ContractDateEnd - today).Days;
                if (dayBefore < 0)
                {
                    message +=
                        $"<li><b>{item.Name}</b> - <b style='color:#a94442'>просрочен</b> ({item.ContractDateEnd.ToShortDateString()})</li>";
                }
                else if (dayBefore == 0)
                {
                    message +=
                        $"<li><b>{item.Name}</b> - сдача <b>сегодня</b> ({item.ContractDateEnd.ToShortDateString()})</li>";
                }
                else
                {
                    string dayBeforeStr;
                    if (dayBefore.ToString().EndsWith("1"))
                    {
                        dayBeforeStr = "день";
                    }
                    else if (dayBefore.ToString().EndsWith("2") ||
                             dayBefore.ToString().EndsWith("3") ||
                             dayBefore.ToString().EndsWith("4"))
                    {
                        dayBeforeStr = "дня";
                    }
                    else
                    {
                        dayBeforeStr = "дней";
                    }

                    message +=
                        $"<li><b>{item.Name}</b> - до сдачи <b>{dayBefore} {dayBeforeStr}</b> ({item.ContractDateEnd.ToShortDateString()})</li>";
                }
            }
            message += "</ul>";
            return message;
        }

        public async Task<IEnumerable<BuildingObject>> GetBuildingObjectWithClosingContracts()
        {
            var today = _dateTimeService.LocalDate;
            var tillDate = today.AddDays(8).AddMilliseconds(-1);

            var items = await _mediator.Send(new ListBuildingObjectsQuery()
            {
                IncludeResponsibleEmployee = true,
                MaxContractDateEnd = tillDate,
                WithStatus = BuildingObjectStatus.Active
            });

            return items.OrderBy(_ => _.ContractDateEnd);
        }
    }
}
