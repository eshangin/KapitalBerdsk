using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using KapitalBerdsk.Web.Classes.Services;

namespace KapitalBerdsk.Web.Classes.Hangfire
{
    public class JobsScheduler
    {
        public void ScheduleStartupJobs()
        {
            RecurringJob.AddOrUpdate<IBuildingObjectClosingContractsChecker>("Check Building Object Closing Contracts",
                (s) => s.Check(),
                "0 4 * * 1-5");
        }
    }
}
