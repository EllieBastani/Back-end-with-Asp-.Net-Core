using System;
using System.Collections.Generic;
using System.Linq;
using Assign2_Server.DataModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Assign2_Server.Data
{
    public class DummyData
    {
        public static async  void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                context.Database.EnsureCreated();
                //context.Database.Migrate();

                // Look for any ailments
                if (context.Users != null && context.Users.Any())
                    return;   // DB has already been seeded

                // adding roles
                List<IdentityRole> roles = new List<IdentityRole>() {
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Child", NormalizedName = "CHILD" }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();

                //adding users
                ApplicationUser santa = new ApplicationUser { UserName = "santa",   Email = "santa@np.com", FirstName = "santa", LastName = "santa",
                    IsNaughty = false, Latitude = 0, Longitude = 0, DateCreated = DateTime.Now, SecurityStamp = Guid.NewGuid().ToString() };
                var result =  await userManager.CreateAsync(santa, "P@$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(santa, "Admin");
                }

                ApplicationUser tim = new ApplicationUser{UserName = "tim",Email = "tim@np.com",FirstName = "Tim",LastName = "Black",
                    IsNaughty = false,Latitude = 12.6654,Longitude = -14.7765,DateCreated = DateTime.Now,SecurityStamp = Guid.NewGuid().ToString()};
                result = await userManager.CreateAsync(tim, "P@$$w0rd");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(tim, "Child");
                }

            }
        }

    }

}