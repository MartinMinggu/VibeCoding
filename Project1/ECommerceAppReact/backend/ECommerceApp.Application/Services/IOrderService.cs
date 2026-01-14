using ECommerceApp.Application.DTOs;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service interface for Order business logic
/// </summary>
public interface IOrderService
{
    Task<CheckoutDto?> GetCheckoutDetailsAsync(string userId, string userAddress);
    Task<int?> PlaceOrderAsync(string userId, PlaceOrderDto dto);
    Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId, string userId);
    Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId);
    Task<IEnumerable<SellerOrderItemDto>> GetSellerOrdersAsync(string sellerId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string status, string sellerId);
    Task<int> GetOrderCountBySellerAsync(string sellerId);
}
