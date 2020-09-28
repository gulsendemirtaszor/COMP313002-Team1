using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "admin";
        private const string adminPassword = "Password0!";

        private const string editorUser = "John";
        private const string editorPassword = "jP@ssw0rd";

        private const string adminRoleName = "Admin";
        private const string editorRoleName = "Editor";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            
            //
            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .GetRequiredService<RoleManager<IdentityRole>>();

            //admin role
            IdentityRole adminRole = await roleManager.FindByNameAsync(adminRoleName);

            if (adminRole == null)
            {
                adminRole = new IdentityRole(adminRoleName);
                await roleManager.CreateAsync(adminRole);
            }

            //editor role
            IdentityRole editorRole = await roleManager.FindByNameAsync(editorRoleName);

            if (editorRole == null)
            {
                editorRole = new IdentityRole(editorRoleName);
                await roleManager.CreateAsync(editorRole);
            }

            //
            UserManager<AppUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<AppUser>>();

            //admin user
            AppUser user = await userManager.FindByNameAsync(adminUser);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = adminUser
                };

                await userManager.CreateAsync(user, adminPassword);
                await userManager.AddToRoleAsync(user, adminRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(user, adminRoleName)))
                {
                    await userManager.AddToRoleAsync(user, adminRoleName);
                }
            }

            //editor user
            AppUser editorUserIdentity = await userManager.FindByNameAsync(editorUser);

            if (editorUserIdentity == null)
            {
                editorUserIdentity = new AppUser
                {
                    UserName = editorUser
                };

                await userManager.CreateAsync(editorUserIdentity, editorPassword);
                await userManager.AddToRoleAsync(editorUserIdentity, editorRoleName);
            }
            else
            {
                if (!(await userManager.IsInRoleAsync(editorUserIdentity, editorRoleName)))
                {
                    await userManager.AddToRoleAsync(editorUserIdentity, editorRoleName);
                }
            }

        }
    }
}
