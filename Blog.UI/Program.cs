using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.UI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Blog.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            try
            {
                var scope = host.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                if(!context.Roles.Any())
                {
                    //create role
                    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                }

                if (!context.Users.Any(u => u.UserName == "admin"))
                {
                    //create user
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@admin.com"
                    };
                    var result = userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
                    userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
