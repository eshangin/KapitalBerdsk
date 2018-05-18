using System;
using System.IO;
using Hangfire;
using KapitalBerdsk.Web.Classes.Data;
using KapitalBerdsk.Web.Classes.Hangfire;
using KapitalBerdsk.Web.Classes.Models;
using KapitalBerdsk.Web.Classes.Options;
using KapitalBerdsk.Web.Classes.Resources;
using KapitalBerdsk.Web.Classes.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KapitalBerdsk.Web.Classes
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password = new PasswordOptions
                    {
                        RequireDigit = true,
                        RequiredLength = 6,
                        RequireLowercase = false,
                        RequireUppercase = false,
                        RequireNonAlphanumeric = false
                    };
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<RuIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.Configure<SmtpOptions>(Configuration.GetSection("SmtpOptions"));
            services.Configure<YandexMetrikaOptions>(Configuration.GetSection("YandexMetrika"));
            services.Configure<GeneralOptions>(Configuration.GetSection("GeneralOptions"));

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IBuildingObjectClosingContractsChecker, BuildingObjectClosingContractsChecker>();
            services.AddTransient<IPingWebAppService, PingWebAppService>();
            services.AddTransient<IPayEmployeePayrollService, PayEmployeePayrollService>();

            var generalOptions = new GeneralOptions();
            Configuration.GetSection("GeneralOptions").Bind(generalOptions);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(generalOptions.DataProtectionKeysPath))
                .SetApplicationName("KapitalBerdsk.Web");

            services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(SharedResource));
                });

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext dbContext)
        {
            dbContext.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
            }

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            var cultureInfo = new System.Globalization.CultureInfo("ru-RU");
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new DashboardAuthorizationFilter() }
            });

            new JobsScheduler().ScheduleStartupJobs();
        }
    }
}
