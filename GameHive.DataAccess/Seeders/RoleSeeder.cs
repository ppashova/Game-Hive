using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Company", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(role));
                }
            }
        }
    }
}
