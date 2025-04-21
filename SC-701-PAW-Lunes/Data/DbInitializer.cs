using Microsoft.AspNetCore.Identity;
using SC_701_PAW_Lunes.Models;

namespace SC_701_PAW_Lunes.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(PAWDbContext context,
                                         UserManager<User> userManager,
                                         RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Create default roles
            string[] roleNames = { "ADMIN", "USER" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@inventory.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    NombreCompleto = "System Administrator",
                    EmailConfirmed = true,
                    Active = true,
                    PasswordRecoveryMode = false
                };

                string adminPassword = "12341234";
                var createAdmin = await userManager.CreateAsync(admin, adminPassword);

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "ADMIN");
                }
            }

            // Create regular user
            var userEmail = "user@inventory.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);

            if (regularUser == null)
            {
                var user = new User
                {
                    UserName = userEmail,
                    Email = userEmail,
                    NombreCompleto = "Regular User",
                    EmailConfirmed = true,
                    Active = true,
                    PasswordRecoveryMode = false
                };

                string userPassword = "12341234";
                var createUser = await userManager.CreateAsync(user, userPassword);

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "USER");
                }
            }
        }
    }
}
