using Ecommerce_Web.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ecommerce_Web.Utility
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(
            IServiceProvider services,
            IConfiguration config,
            ILogger? logger = null)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            // seed roles...
            foreach (var role in DefaultRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // seed admin...
            var email = config["DefaultAdmin:Email"];
            var userName = config["DefaultAdmin:UserName"] ?? email;
            var password = config["DefaultAdmin:Password"];

            logger?.LogInformation("Seeding admin:{Email}", email);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger?.LogWarning("Missing DefaultAdmin config. Skip admin seeding.");
                return;
            }

            var admin = await userManager.FindByEmailAsync(email);
            if (admin == null)
            {
                logger?.LogInformation("Admin not found -> creating");

                admin = new ApplicationUser 
                {
                    Email = email, 
                    UserName = userName, 
                    EmailConfirmed = true 
                };

                var create = await userManager.CreateAsync(admin, password);
                if (!create.Succeeded)
                    throw new Exception(string.Join("; ", create.Errors.Select(e => e.Description)));
            } else 
            {
                logger?.LogInformation("Admin found -> checking/resetting password");

                var ok = await userManager.CheckPasswordAsync(admin, password);
                if (!ok)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(admin);
                    var reset = await userManager.ResetPasswordAsync(admin, token, password);

                    if (!reset.Succeeded)
                    {
                        var err = string.Join("; ", reset.Errors.Select(e => e.Description));
                        throw new Exception($"Reset admin password failed: {err}");
                    }
                }

                // (tuỳ chọn) mở khoá + confirm email cho tiện dev
                admin.EmailConfirmed = true;
                admin.LockoutEnd = null;
                admin.AccessFailedCount = 0;
                await userManager.UpdateAsync(admin);
            }


            if (!await userManager.IsInRoleAsync(admin, Roles.Admin))
            {
                var addRole = await userManager.AddToRoleAsync(admin, Roles.Admin);
                if (!addRole.Succeeded)
                    throw new Exception(string.Join("; ", addRole.Errors.Select(e => e.Description)));
            }
        }
    }
}
