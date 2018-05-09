using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using System.Web;

namespace KapitalBerdsk.Web.Classes.Hangfire
{
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // if unknown, assume not local
            if (String.IsNullOrEmpty(context.Request.RemoteIpAddress))
                return false;

            // check if localhost
            if (context.Request.RemoteIpAddress == "127.0.0.1" || context.Request.RemoteIpAddress == "::1")
                return true;

            // compare with local address
            if (context.Request.RemoteIpAddress == context.Request.LocalIpAddress)
                return true;

            return httpContext.User.Identity.IsAuthenticated &&
                httpContext.User.IsInRole(Constants.Roles.Admin);
        }
    }
}
