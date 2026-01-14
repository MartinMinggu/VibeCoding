using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Application.Services;
using ECommerceApp.Application.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Place a new order
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            if (string.IsNullOrEmpty(dto.ShippingAddress))
                return BadRequest(new { message = "Shipping address is required" });

            if (string.IsNullOrEmpty(dto.PaymentMethod))
                return BadRequest(new { message = "Payment method is required" });

            var orderId = await _orderService.PlaceOrderAsync(userId, dto);
            
            if (orderId == null)
                return BadRequest(new { message = "Failed to place order. Cart might be empty or items out of stock." });

            return Ok(new { orderId = orderId, message = "Order placed successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error placing order", error = ex.Message });
        }
    }

    /// <summary>
    /// Get current user's orders
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving orders", error = ex.Message });
        }
    }

    /// <summary>
    /// Get order details by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetails(int id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var order = await _orderService.GetOrderDetailsAsync(id, userId);
            
            if (order == null)
                return NotFound(new { message = "Order not found or access denied" });

            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving order details", error = ex.Message });
        }
    }

    /// <summary>
    /// Get checkout details (preview)
    /// </summary>
    [HttpGet("checkout")]
    public async Task<IActionResult> GetCheckoutDetails()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            // For now we pass empty address, in real app we might fetch user's saved address
            var details = await _orderService.GetCheckoutDetailsAsync(userId, "");
            
            if (details == null)
                return BadRequest(new { message = "Cart is empty" });

            return Ok(details);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving checkout details", error = ex.Message });
        }
    }
    
    /// <summary>
    /// Get seller orders
    /// </summary>
    [HttpGet("seller")]
    public async Task<IActionResult> GetSellerOrders()
    {
        try
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // If checking as "guest-user", this won't work well for seller features, but we'll keep consistent
            if (string.IsNullOrEmpty(sellerId)) sellerId = "guest-user"; 
            
            var orders = await _orderService.GetSellerOrdersAsync(sellerId);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving seller orders", error = ex.Message });
        }
    }

    /// <summary>
    /// Update order status (Seller only)
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateStatusRequest request)
    {
        try
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var result = await _orderService.UpdateOrderStatusAsync(id, request.Status, sellerId);
            
            if (!result)
                return BadRequest(new { message = "Failed to update status. Order not found or you don't have permission." });

            return Ok(new { message = "Order status updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating order status", error = ex.Message });
        }
    }
}

public class UpdateStatusRequest
{
    public string Status { get; set; } = string.Empty;
}
