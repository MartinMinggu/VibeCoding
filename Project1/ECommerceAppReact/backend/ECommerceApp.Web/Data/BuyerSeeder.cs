using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Web.Data;

public static class BuyerSeeder
{
    public static async Task SeedBuyersAsync(UserManager<ApplicationUser> userManager)
    {
        var buyers = new[]
        {
            new { Email = "buyer1@test.com", FirstName = "Ahmad", LastName = "Buyer" },
            new { Email = "buyer2@test.com", FirstName = "Siti", LastName = "Customer" },
            new { Email = "buyer3@test.com", FirstName = "Budi", LastName = "Shopper" },
            new { Email = "testbuyer@test.com", FirstName = "Test", LastName = "Buyer" }
        };

        foreach (var buyerData in buyers)
        {
            var existingUser = await userManager.FindByEmailAsync(buyerData.Email);
            if (existingUser == null)
            {
                var buyer = new ApplicationUser
                {
                    UserName = buyerData.Email,
                    Email = buyerData.Email,
                    FirstName = buyerData.FirstName,
                    LastName = buyerData.LastName,
                    EmailConfirmed = true,
                    IsSeller = false // PEMBELI
                };

                var result = await userManager.CreateAsync(buyer, "Buyer123!");
                if (result.Succeeded)
                {
                    Console.WriteLine($"✅ Buyer created: {buyerData.Email}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create buyer {buyerData.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
