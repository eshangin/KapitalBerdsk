using System;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Models;
using KapitalBerdsk.Web.Classes.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KapitalBerdsk.Web.Classes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var dbInitializerLogger = services.GetRequiredService<ILogger<DbInitializer>>();
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                var payEmployeePayrollService = services.GetService<IPayEmployeePayrollService>();

                try
                {
                    new DbInitializer().Initialize(context, userManager, roleManager, 
                        dbInitializerLogger, payEmployeePayrollService).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    if (builderContext.HostingEnvironment.IsDevelopment())
                    {
                        config.AddJsonFile("appsettings.user.json", optional: true);
                    }
                })
                .UseStartup<Startup>()
                .Build();
    }
}
