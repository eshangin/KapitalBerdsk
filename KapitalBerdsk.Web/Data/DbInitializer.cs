using KapitalBerdsk.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KapitalBerdsk.Web.Data
{
    public class DbInitializer
    {
        public async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            if (context.Users.Any())
            {
                // DB has been seeded
                return;
            }

            await CreateDefaultUser(userManager, logger);
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
