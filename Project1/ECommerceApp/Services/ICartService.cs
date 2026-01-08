using ECommerceApp.DTOs;

namespace ECommerceApp.Services;

/// <summary>
/// Service interface for Cart business logic
/// </summary>
public interface ICartService
{
    Task<CartDto> GetUserCartAsync(string userId);
    Task<bool> AddToCartAsync(string userId, int productId, int quantity);
    Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity, string userId);
    Task<bool> RemoveFromCartAsync(int cartItemId, string userId);
    Task ClearCartAsync(string userId);
}
