using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Order entity
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<Order> AddAsync(Order entity)
    {
        await _context.Orders.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Order entity)
    {
        _context.Orders.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Order entity)
    {
        _context.Orders.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Orders.AnyAsync(o => o.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<IEnumerable<OrderItem>> GetOrderItemsBySellerAsync(string sellerId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Order)
            .ThenInclude(o => o != null ? o.User : null)
            .Include(oi => oi.Product)
            .Where(oi => oi.Product != null && oi.Product.SellerId == sellerId)
            .OrderByDescending(oi => oi.Order != null ? oi.Order.OrderDate : DateTime.MinValue)
            .ToListAsync();
    }

    public async Task<int> GetOrderCountBySellerAsync(string sellerId)
    {
        return await _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.Product != null && oi.Product.SellerId == sellerId)
            .Select(oi => oi.OrderId)
            .Distinct()
            .CountAsync();
    }
}
