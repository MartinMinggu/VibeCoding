using ECommerceApp.Models;

namespace ECommerceApp.Repositories;

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
