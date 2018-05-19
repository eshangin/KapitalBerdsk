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
                IList<ApplicationUser> managers = await _userManager.GetUsersInRoleAsync(Constants.Roles.Manager);
                IEnumerable<string> emails = managers.Select(m => m.Email);
                string message = BuildMessageBody(items);
                foreach (string email in emails)
                {
                    await _emailSender.AddPendingEmail(email, "Окончание контрактов", message);
                }
            }
        }

        private string BuildMessageBody(List<BuildingObject> buildingObjectsWithClosingContracts)
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
                from bo in _context.BuildingObjects
                where bo.Status == BuildingObjectStatus.Active &&
                      bo.ContractDateEnd >= today &&
                      bo.ContractDateEnd <= tillDate
                orderby bo.ContractDateEnd
                select bo).ToListAsync();

            return items;
        }
    }
}
