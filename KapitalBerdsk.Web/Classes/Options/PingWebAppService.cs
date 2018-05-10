using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace KapitalBerdsk.Web.Classes.Options
{
    public class PingWebAppService : IPingWebAppService
    {
        private readonly GeneralOptions _generalOptions;

        public PingWebAppService(IOptions<GeneralOptions> opts)
        {
            _generalOptions = opts.Value;
        }

        public void Ping()
        {
            var client = new WebClient();
            client.DownloadString(_generalOptions.AppBaseUrl + "/api/echo");
        }
    }
}
