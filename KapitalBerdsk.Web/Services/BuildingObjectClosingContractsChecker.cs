using KapitalBerdsk.Web.Classes;
using KapitalBerdsk.Web.Data;
using KapitalBerdsk.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Services
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
            var today = DateTime.UtcNow.AddHours(7).Date;
            var tillDate = today.AddDays(7);

            var items = await (from bo in _context.BuildingObjects
                               where !bo.IsClosed &&
                                   bo.ContractDateEnd >= today &&
                                   bo.ContractDateEnd <= tillDate
                               orderby bo.ContractDateEnd
                               select new
                               {
                                   bo.Name,
                                   bo.ContractDateEnd
                               }).ToListAsync();

            if (items.Any())
            {
                var managers = await _userManager.GetUsersInRoleAsync(Constants.Roles.Manager);
                var emails = managers.Select(m => m.Email);
                string message = "<h3>Приближается срок окончания контрактов:</h3><ul>";
                foreach (var item in items)
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
                await _emailSender.SendEmailAsync(emails, "Окончание контрактов", message);
            }
        }
    }
}
