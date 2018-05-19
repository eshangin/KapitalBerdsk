using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Services
{
    public class BuildingObjectClosingContractsChecker : IBuildingObjectClosingContractsChecker
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuildingObjectClosingContractsChecker(
            IEmailSender emailSender,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _emailSender = emailSender;
            _context = context;
            _userManager = userManager;
        }

        public async Task Check()
        {
            DateTime today = DateTime.UtcNow.AddHours(7).Date;

            List<BuildingObject> items = await GetBuildingObjectWithClosingContracts();

            if (items.Any())
            {
                const string messageSubject = "Окончание контрактов";
                IList<ApplicationUser> managers = await _userManager.GetUsersInRoleAsync(Constants.Roles.Manager);
                IEnumerable<Employee> responsibleEmployees = from item in items
                                                             where item.ResponsibleEmployee != null
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
            DateTime today = DateTime.UtcNow.AddHours(7).Date;

            string message = "<h3>Приближается срок окончания контрактов:</h3><ul>";
            foreach (BuildingObject item in buildingObjectsWithClosingContracts)
            {
                int dayBefore = (item.ContractDateEnd - today).Days;
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
                message += $"<li><b>{item.Name}</b> - до сдачи <b>{dayBefore} {dayBeforeStr}</b> ({item.ContractDateEnd.ToShortDateString()})</li>";
            }
            message += "</ul>";
            return message;
        }

        public async Task<List<BuildingObject>> GetBuildingObjectWithClosingContracts()
        {
            var today = DateTime.UtcNow.AddHours(7).Date;
            var tillDate = today.AddDays(7);

            var items = await (
                from bo in _context.BuildingObjects.Include(item => item.ResponsibleEmployee)
                where bo.Status == BuildingObjectStatus.Active &&
                      bo.ContractDateEnd >= today &&
                      bo.ContractDateEnd <= tillDate
                orderby bo.ContractDateEnd
                select bo).ToListAsync();

            return items;
        }
    }
}
