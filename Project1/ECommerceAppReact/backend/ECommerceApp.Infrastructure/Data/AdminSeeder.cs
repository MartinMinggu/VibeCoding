using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Infrastructure.Data;

public static class AdminSeeder
{
    public static async Task SeedSuperAdminAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Define roles
        string[] roles = { "SuperAdmin", "Admin", "Seller", "Customer" };

        // Create roles if they don't exist
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create SuperAdmin user if not exists
        const string superAdminEmail = "superadmin@ecommerce.com";
        const string superAdminPassword = "SuperAdmin123!";

        var superAdmin = await userManager.FindByEmailAsync(superAdminEmail);
        if (superAdmin == null)
        {
            superAdmin = new ApplicationUser
            {
                UserName = superAdminEmail,
                Email = superAdminEmail,
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                IsSeller = false
            };

            var result = await userManager.CreateAsync(superAdmin, superAdminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                Console.WriteLine("✅ SuperAdmin account created successfully!");
                Console.WriteLine($"   Email: {superAdminEmail}");
                Console.WriteLine($"   Password: {superAdminPassword}");
            }
            else
            {
                Console.WriteLine("❌ Failed to create SuperAdmin: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            Console.WriteLine("ℹ️ SuperAdmin account already exists.");
        }
    }
}
