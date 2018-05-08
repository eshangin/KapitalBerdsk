using Hangfire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace KapitalBerdsk.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private BackgroundJobServer _server;

        public Service1()
        {
            InitializeComponent();

            System.Threading.Thread.Sleep(10000);

            GlobalConfiguration.Configuration.UseSqlServerStorage(
                System.Configuration.ConfigurationManager.ConnectionStrings["Hangfire.ConnectionString"].ConnectionString);
        }

        protected override void OnStart(string[] args)
        {
            _server = new BackgroundJobServer();

            var timezone = TimeZoneInfo.CreateCustomTimeZone("nsk", new TimeSpan(07, 00, 00), "(GMT+07:00) Nsk", "nsk");

            RecurringJob.AddOrUpdate("Check Building Object Closing Contracts", 
                () => Console.WriteLine("Daily Job"),
                "0 0 10 ? * MON-FRI *",
                timezone);
        }

        protected override void OnStop()
        {
            _server.Dispose();
        }
    }
}
