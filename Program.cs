using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Remood_Place.Data;
using Remood_Place.Models;

namespace Remood_Place
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie().AddGoogle(GoogleDefaults.AuthenticationScheme, option =>
            {
                //option.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                //option.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // httponly cookie
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "application.Identity";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Error/Index/403";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = new IdentityUser
                {
                    UserName = "Super@gmail.com",
                    Email = "Super@gmail.com",
                    PhoneNumber = "1234567890",
                };
                string userPWD = "Super123_";
                var createUser = await userManager.CreateAsync(user, userPWD);

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = new IdentityUser
                {
                    UserName = "Admin@gmail.com",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "1234567890",

                };
                string userPWD = "Admin123_";
                var createUser = await userManager.CreateAsync(user, userPWD);

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var user = new IdentityUser
                {
                    UserName = "User@gmail.com",
                    Email = "User@gmail.com",
                    PhoneNumber = "1234567890",

                };
                string userPWD = "User123_";
                var createUser = await userManager.CreateAsync(user, userPWD);

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
            }
            app.Run();
        }
    }
}
