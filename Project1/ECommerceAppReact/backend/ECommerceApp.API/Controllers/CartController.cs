using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Application.Services;
using ECommerceApp.Application.DTOs;
using System.Security.Claims;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Get current user's cart
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        try
        {
            // TODO: Get userId from authenticated user claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving cart", error = ex.Message });
        }
    }

    /// <summary>
    /// Add product to cart
    /// </summary>
    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var result = await _cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);
            
            if (!result)
                return BadRequest(new { message = "Failed to add item to cart" });

            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding to cart", error = ex.Message });
        }
    }

    /// <summary>
    /// Update cart item quantity
    /// </summary>
    [HttpPut("items/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemRequest request)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var result = await _cartService.UpdateCartItemQuantityAsync(cartItemId, request.Quantity, userId);
            
            if (!result)
                return NotFound(new { message = "Cart item not found" });

            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating cart item", error = ex.Message });
        }
    }

    /// <summary>
    /// Remove item from cart
    /// </summary>
    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            var result = await _cartService.RemoveFromCartAsync(cartItemId, userId);
            
            if (!result)
                return NotFound(new { message = "Cart item not found" });

            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error removing from cart", error = ex.Message });
        }
    }

    /// <summary>
    /// Clear entire cart
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "guest-user";
            
            await _cartService.ClearCartAsync(userId);
            return Ok(new { message = "Cart cleared successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error clearing cart", error = ex.Message });
        }
    }
}

// Request DTOs
public class AddToCartRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; } = 1;
}

public class UpdateCartItemRequest
{
    public int Quantity { get; set; }
}
