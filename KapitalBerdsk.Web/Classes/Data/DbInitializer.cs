using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Models;
using KapitalBerdsk.Web.Classes.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class DbInitializer
    {
        public async Task Initialize(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger logger,
            IPayEmployeePayrollService payEmployeePayrollService)
        {
            if (!context.Users.Any())
            {
                await CreateDefaultUser(userManager, logger);
            }

            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Manager));

                List<ApplicationUser> users = await context.Users.ToListAsync();
                foreach (var u in users)
                {
                    if (u.Email == "admin@admin.com")
                    {
                        await userManager.AddToRoleAsync(u, Constants.Roles.Admin);
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(u, Constants.Roles.Manager);
                    }
                }
            }

            if (!context.EmployeePayrolls.Any())
            {
                await payEmployeePayrollService.PayToAllEmployees();
            }
        }

        private async Task CreateDefaultUser(UserManager<ApplicationUser> userManager, ILogger logger)
        {
            var user = new ApplicationUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",                
            };
            const string defaultPwd = "abcdefg1";

            var result = await userManager.CreateAsync(user, defaultPwd);
            
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                logger.LogWarning($"Default user wasn't created. Error: {error.Code}, {error.Description}");
            }
        }
    }
}
