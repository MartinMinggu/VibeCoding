using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Cart entity
/// </summary>
public interface ICartRepository : IRepository<Cart>
{
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart?> GetCartWithItemsAsync(string userId);
    Task<CartItem?> GetCartItemAsync(int cartId, int productId);
    Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
    Task AddCartItemAsync(CartItem cartItem);
    Task UpdateCartItemAsync(CartItem cartItem);
    Task RemoveCartItemAsync(CartItem cartItem);
    Task RemoveCartItemsAsync(IEnumerable<CartItem> cartItems);
}
