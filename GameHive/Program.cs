using Microsoft.EntityFrameworkCore;
using GameHive.DataAccess;
using GameHive.Core.IServices;
using GameHive.Core.Services;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Identity;
using GameHive.DataAccess.Repository.Repositories;
using GameHive.DataAccess.Repository.IRepositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using GameHive.DataAccess.Repository;
using GameHive.DataAccess.Seeders;
using SendGrid.Helpers.Mail;
using CloudinaryDotNet.Core;
using CloudinaryDotNet;
using GameHive.Utilities;
namespace GameHive
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<IGameTagRepository, GameTagRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IGameTagService, GameTagService>();
            builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("GameHive.DataAccess")));

            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.ConfigureApplicationCookie(o =>
            {
                o.ExpireTimeSpan = TimeSpan.FromDays(5);
                o.SlidingExpiration = true;
            });

            builder.Services.AddScoped<CloudinaryService>();
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
            builder.Services.AddScoped<CloudinaryService>();


            builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.Tokens.ProviderMap.Add("CustomEmailConfirmation",
                    new TokenProviderDescriptor(
                        typeof(CustomEmailConfirmationTokenProvider<IdentityUser>)));
                config.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();


            builder.Services.AddTransient<CustomEmailConfirmationTokenProvider<IdentityUser>>();
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            builder.Services.ConfigureApplicationCookie(options => { options.LoginPath = "/Identity/Account/Login"; options.AccessDeniedPath = "/Identity/Account/AccessDenied"; });



            // Add session services
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
                options.Cookie.HttpOnly = true; // Make the cookie HttpOnly for security
                options.Cookie.IsEssential = true; // Make it essential for session tracking
            });
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                await RoleSeeder.SeedAsync(roleManager);
                await AdminSeeder.SeedAdminAsync(userManager);
                await AdminSeeder.SeedCompanyAsync(userManager);
            }
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                CreateRoles(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                context.Database.Migrate();
                DbInitializer.Seed(context);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
            app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "User", "Company" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

    }
}
