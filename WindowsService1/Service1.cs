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

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        private BackgroundJobServer _server;

        public Service1()
        {
            InitializeComponent();

            GlobalConfiguration.Configuration.UseSqlServerStorage(
                System.Configuration.ConfigurationManager.ConnectionStrings[""].ConnectionString);
        }

        protected override void OnStart(string[] args)
        {
            _server = new BackgroundJobServer();
        }

        protected override void OnStop()
        {
            _server.Dispose();
        }
    }
}
