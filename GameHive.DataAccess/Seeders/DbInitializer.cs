using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.Migrate();

            TagSeeder.Seed(context);
            GameSeeder.Seed(context);
            GameTagSeeder.Seed(context);
            GameImageSeeder.Seed(context);
        }
    }
}
