using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Web.Data;

/// <summary>
/// Seeds 5 new seller accounts with Indonesian store names
/// </summary>
public static class SellerSeeder
{
    public static async Task<List<string>> SeedSellersAsync(UserManager<ApplicationUser> userManager)
    {
        var sellerIds = new List<string>();
        
        var sellers = new[]
        {
            new { FirstName = "Budi", LastName = "Elektronik", Email = "toko.elektronik.jaya@example.com", StoreName = "Toko Elektronik Jaya" },
            new { FirstName = "Sari", LastName = "Fashion", Email = "butik.fashion.nusantara@example.com", StoreName = "Butik Fashion Nusantara" },
            new { FirstName = "Ahmad", LastName = "Cerdas", Email = "toko.buku.cerdas@example.com", StoreName = "Toko Buku Cerdas" },
            new { FirstName = "Dewi", LastName = "Dekor", Email = "rumah.dekor.indonesia@example.com", StoreName = "Rumah Dekor Indonesia" },
            new { FirstName = "Rudi", LastName = "Sport", Email = "sport.center.official@example.com", StoreName = "Sport Center Official" },
        };

        foreach (var seller in sellers)
        {
            var existingUser = await userManager.FindByEmailAsync(seller.Email);
            
            if (existingUser != null)
            {
                sellerIds.Add(existingUser.Id);
                continue;
            }

            var user = new ApplicationUser
            {
                UserName = seller.Email,
                Email = seller.Email,
                FirstName = seller.FirstName,
                LastName = seller.LastName,
                IsSeller = true,
                Address = $"{seller.StoreName}, Jakarta, Indonesia",
                EmailConfirmed = true,
                CreatedAt = DateTime.Now.AddDays(-Random.Shared.Next(30, 365))
            };

            var result = await userManager.CreateAsync(user, "Seller123!");
            
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Seller");
                sellerIds.Add(user.Id);
            }
        }

        return sellerIds;
    }
}
