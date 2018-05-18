using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using KapitalBerdsk.Web.Classes.Options;
using KapitalBerdsk.Web.Classes.Services;

namespace KapitalBerdsk.Web.Classes.Hangfire
{
    public class JobsScheduler
    {
        public void ScheduleStartupJobs()
        {
            RecurringJob.AddOrUpdate<IBuildingObjectClosingContractsChecker>("Check Building Object Closing Contracts",
                (s) => s.Check(),
                "0 3 * * 1-5");

            RecurringJob.AddOrUpdate<IPingWebAppService>("Ping web app request",
                (service) => service.Ping(),
                "0/5 * * * *");

            RecurringJob.AddOrUpdate<IPayEmployeePayrollService>("Pay employee payroll",
                (service) => service.PayToAllEmployees(),
                "0 0 1 * *");
        }
    }
}
