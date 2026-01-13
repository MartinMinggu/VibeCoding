using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Cart entity
/// </summary>
public class CartRepository : ICartRepository
{
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByIdAsync(int id)
    {
        return await _context.Carts.FindAsync(id);
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        return await _context.Carts.ToListAsync();
    }

    public async Task<Cart> AddAsync(Cart entity)
    {
        await _context.Carts.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Cart entity)
    {
        _context.Carts.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Cart entity)
    {
        _context.Carts.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Carts.AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<Cart?> GetCartWithItemsAsync(string userId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem?> GetCartItemAsync(int cartId, int productId)
    {
        return await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
    {
        return await _context.CartItems.FindAsync(cartItemId);
    }

    public async Task AddCartItemAsync(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public async Task UpdateCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Update(cartItem);
        await Task.CompletedTask;
    }

    public async Task RemoveCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
        await Task.CompletedTask;
    }

    public async Task RemoveCartItemsAsync(IEnumerable<CartItem> cartItems)
    {
        _context.CartItems.RemoveRange(cartItems);
        await Task.CompletedTask;
    }
}
