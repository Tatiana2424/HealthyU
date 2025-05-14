using HealthyU.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace HealthyU.WebApi.Extensions
{
    public static class SeedRolesExtensions
    {
        public static async Task EnsureRolesAndAdminAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            foreach (var role in new[] { "user", "admin" })
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole<int>(role));
            }

            const string adminUserName = "admin";
            const string adminEmail = "admin@healthyu.local";

            var existing = await userMgr.FindByNameAsync(adminUserName)
                           ?? await userMgr.FindByEmailAsync(adminEmail);

            if (existing == null)
            {
                var admin = new User
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    FirstName = "System",
                    LastName = "Administrator"
                };

                var password = "Admin@123";
                var result = await userMgr.CreateAsync(admin, password);

                if (result.Succeeded)
                    await userMgr.AddToRoleAsync(admin, "admin");
                else
                    throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

    }
}
