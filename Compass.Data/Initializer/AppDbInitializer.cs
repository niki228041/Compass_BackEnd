using Compass.Data.Data.Context;
using Compass.Data.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compass.Data.Initializer
{
    public class AppDbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                UserManager<AppUser> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                if (userManager.FindByNameAsync("master").Result == null)
                {
                    AppUser admin = new AppUser()
                    {
                        UserName = "master",
                        Surname = "Snow",
                        Name = "John",
                        Email = "lopocznikita@gmail.com",
                        EmailConfirmed = true,
                        Role = "master"
                    };

                    AppUser user = new AppUser()
                    {
                        UserName = "user",
                        Email = "user@email.com",
                        EmailConfirmed = true,
                        Name = "Bart",
                        Surname = "Simpson",
                        Role = "user"
                    };

                    context.Roles.AddRange(
                        new IdentityRole()
                        {
                            Name = "master",
                            NormalizedName = "MASTER"
                        },
                        new IdentityRole()
                        {
                            Name = "user",
                            NormalizedName = "USER"
                        });

                    await context.SaveChangesAsync();

                    IdentityResult resultadmin = userManager.CreateAsync(admin, "Qwerty-1").Result;
                    IdentityResult resultuser = userManager.CreateAsync(user, "Qwerty-1").Result;

                    if (resultadmin.Succeeded)
                    {
                        userManager.AddToRoleAsync(admin, "master").Wait();
                    }
                    if (resultuser.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "user").Wait();
                    }
                }
            }

        }
    }
}
