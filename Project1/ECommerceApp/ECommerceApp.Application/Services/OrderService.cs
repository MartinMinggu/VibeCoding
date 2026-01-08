using ECommerceApp.Application.DTOs;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service implementation for Order business logic
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CheckoutDto?> GetCheckoutDetailsAsync(string userId, string userAddress)
    {
        var cart = await _cartRepository.GetCartWithItemsAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            return null;

        var cartItems = cart.CartItems.Select(ci => new CartItemDto
        {
            Id = ci.Id,
            ProductId = ci.ProductId,
            ProductName = ci.Product?.Name ?? string.Empty,
            ProductImageUrl = ci.Product?.ImageUrl ?? string.Empty,
            UnitPrice = ci.Product?.Price ?? 0,
            Quantity = ci.Quantity,
            AvailableStock = ci.Product?.Stock ?? 0
        }).ToList();

        return new CheckoutDto
        {
            CartItems = cartItems,
            TotalAmount = cartItems.Sum(i => i.Subtotal),
            ShippingAddress = userAddress
        };
    }

    public async Task<int?> PlaceOrderAsync(string userId, PlaceOrderDto dto)
    {
        var cart = await _cartRepository.GetCartWithItemsAsync(userId);
        if (cart == null || !cart.CartItems.Any())
            return null;

        // Validate stock availability
        foreach (var item in cart.CartItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null || !product.IsActive || product.Stock < item.Quantity)
                return null;
        }

        // Create order
        var order = new Order
        {
            UserId = userId,
            ShippingAddress = dto.ShippingAddress,
            PaymentMethod = dto.PaymentMethod,
            TotalAmount = cart.CartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity),
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product?.Price ?? 0
            }).ToList()
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        // Update stock
        foreach (var item in cart.CartItems)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock -= item.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
        await _productRepository.SaveChangesAsync();

        // Clear cart
        await _cartRepository.RemoveCartItemsAsync(cart.CartItems);
        await _cartRepository.SaveChangesAsync();

        return order.Id;
    }

    public async Task<OrderDetailsDto?> GetOrderDetailsAsync(int orderId, string userId)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null || order.UserId != userId)
            return null;

        return new OrderDetailsDto
        {
            Id = order.Id,
            OrderNumber = $"ORD-{order.Id:D6}",
            OrderDate = order.OrderDate,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            PaymentMethod = order.PaymentMethod,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? string.Empty,
                ProductImageUrl = oi.Product?.ImageUrl ?? string.Empty,
                UnitPrice = oi.UnitPrice,
                Quantity = oi.Quantity
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(string userId)
    {
        var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            OrderNumber = $"ORD-{o.Id:D6}",
            OrderDate = o.OrderDate,
            Status = o.Status,
            TotalAmount = o.TotalAmount,
            ItemCount = o.OrderItems.Count,
            Items = o.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? string.Empty,
                ProductImageUrl = oi.Product?.ImageUrl ?? string.Empty,
                UnitPrice = oi.UnitPrice,
                Quantity = oi.Quantity
            }).ToList()
        });
    }

    public async Task<IEnumerable<SellerOrderItemDto>> GetSellerOrdersAsync(string sellerId)
    {
        var orderItems = await _orderRepository.GetOrderItemsBySellerAsync(sellerId);
        return orderItems.Select(oi => new SellerOrderItemDto
        {
            OrderId = oi.OrderId,
            OrderNumber = $"ORD-{oi.OrderId:D6}",
            OrderDate = oi.Order?.OrderDate ?? DateTime.MinValue,
            Status = oi.Order?.Status ?? string.Empty,
            ShippingAddress = oi.Order?.ShippingAddress ?? string.Empty,
            PaymentMethod = oi.Order?.PaymentMethod ?? string.Empty,
            CustomerName = oi.Order?.User != null 
                ? $"{oi.Order.User.FirstName} {oi.Order.User.LastName}" 
                : string.Empty,
            ProductName = oi.Product?.Name ?? string.Empty,
            Quantity = oi.Quantity,
            UnitPrice = oi.UnitPrice
        });
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status, string sellerId)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
        if (order == null)
            return false;

        // Verify seller owns at least one product in this order
        var hasSellerProduct = order.OrderItems.Any(oi => 
            oi.Product != null && oi.Product.SellerId == sellerId);
        
        if (!hasSellerProduct)
            return false;

        order.Status = status;
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();

        return true;
    }

    public async Task<int> GetOrderCountBySellerAsync(string sellerId)
    {
        return await _orderRepository.GetOrderCountBySellerAsync(sellerId);
    }
}
