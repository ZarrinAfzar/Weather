using Weather.Data.Base;
using Weather.Data.Enums;
using Weather.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.DBInitializer
{
    public static class Initializer
    {
        //public static async Task CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
        //    string[] roleNames = { "AdminiStratore", "Admin", "User" };//,Enum.GetNames(typeof(EnuRole));
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            roleResult = await RoleManager.CreateAsync(new Role(roleName));
        //        }
        //    }

        //    var user = await UserManager.FindByEmailAsync("Admin@gmail.com");

        //    if (user == null)
        //    {
        //        user = new User()
        //        {
        //            UserName = "Admin@gmail.com", 
        //            Email = "Admin@gmail.com",
        //        };
        //        await UserManager.CreateAsync(user, "Admin@123");
        //    }
        //    await UserManager.AddToRoleAsync(user, "AdminiStratore");
        //}
        //DataBaseContext context = serviceProvider.GetRequiredService<DataBaseContext>();
        //if (!context.Database.EnsureCreated())
        //    context.Database.Migrate();
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var Roles = Enum.GetValues(typeof(EnuRole));
            IdentityResult roleResult;

            foreach (Enum role in Roles)
            {
                string rolevalue = Enum.GetName(typeof(EnuRole), role);
                string roleName = role.GetDisplayName();
                var roleExist = await RoleManager.RoleExistsAsync(rolevalue);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new Role(rolevalue, roleName));
                }
            }
            var user = await UserManager.FindByEmailAsync("Admin@gmail.com");

            if (user == null)
            {
                user = new User()
                {
                    Registered = true,
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                };
                await UserManager.CreateAsync(user, "Admin@123");
            }
            await UserManager.AddToRoleAsync(user, "AdminiStratore");
        }
    }
}
