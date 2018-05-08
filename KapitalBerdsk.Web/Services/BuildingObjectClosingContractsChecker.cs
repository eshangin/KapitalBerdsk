using KapitalBerdsk.Web.Data;
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

        public BuildingObjectClosingContractsChecker(
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public void Check()
        {
            var today = DateTime.UtcNow.AddHours(7).Date;
            var tillDate = today.AddDays(7);

            var items = (from bo in _context.BuildingObjects
                              where !bo.IsClosed &&
                                  bo.ContractDateEnd >= today &&
                                  bo.ContractDateEnd <= tillDate
                              orderby bo.ContractDateEnd
                              select new
                              {
                                  bo.Name,
                                  bo.ContractDateEnd
                              }).ToList();

            if (items.Any())
            {
                //_emailSender.SendEmailAsync("", "", "");
            }
        }
    }
}
