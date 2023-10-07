using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using AnyoneForTennis.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        InitializeRoles(roleManager);
        InitializeUsers(userManager);
    }

    public static void InitializeRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Coach", "Member" };

        foreach (var roleName in roleNames)
        {
            if (!roleManager.RoleExistsAsync(roleName).Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = roleName
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }

    public static void InitializeUsers(UserManager<ApplicationUser> userManager)
    {
        if (userManager.FindByEmailAsync("admin@e.com").Result == null)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = "admin@e.com",
                Email = "admin@e.com",
                FirstName = "Admin",
                LastName = "User",
                Biography = "Bio for Admin User",
                EmailConfirmed = true
            };

            IdentityResult result = userManager.CreateAsync(user, "Admin0.").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}
