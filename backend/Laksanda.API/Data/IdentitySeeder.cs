using Laksanda.API.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Laksanda.API.Data;

public static class IdentitySeeder
{
    private static readonly string[] DefaultRoles = ["Admin", "User"];

    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        foreach (var roleName in DefaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpperInvariant()
                });
            }
        }
    }
}