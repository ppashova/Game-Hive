using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@example.com";
            var adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        public static async Task SeedCompanyAsync(UserManager<IdentityUser> userManager)
        {
            var companyEmail = "company@example.com";
            var companyPassword = "Company123!";

            var companyUser = await userManager.FindByEmailAsync(companyEmail);
            if (companyUser == null)
            {
                companyUser = new IdentityUser
                {
                    UserName = companyEmail,
                    Email = companyEmail,
                    EmailConfirmed = true
                };
                var createResult = await userManager.CreateAsync(companyUser, companyPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(companyUser, "Company");
                }
            }
        }
    }
}
