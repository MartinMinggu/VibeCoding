using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Order entity
/// </summary>
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task<Order?> GetOrderWithDetailsAsync(int orderId);
    Task<IEnumerable<OrderItem>> GetOrderItemsBySellerAsync(string sellerId);
    Task<int> GetOrderCountBySellerAsync(string sellerId);
}
