using ECommerceApp.DTOs;
using ECommerceApp.Models;
using ECommerceApp.Repositories;

namespace ECommerceApp.Services;

/// <summary>
/// Service implementation for Cart business logic
/// </summary>
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<CartDto> GetUserCartAsync(string userId)
    {
        var cart = await _cartRepository.GetCartWithItemsAsync(userId);
        
        if (cart == null)
        {
            return new CartDto { UserId = userId };
        }

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.CartItems.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name ?? string.Empty,
                ProductImageUrl = ci.Product?.ImageUrl ?? string.Empty,
                UnitPrice = ci.Product?.Price ?? 0,
                Quantity = ci.Quantity,
                AvailableStock = ci.Product?.Stock ?? 0
            }).ToList()
        };
    }

    public async Task<bool> AddToCartAsync(string userId, int productId, int quantity)
    {
        // Validate product
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null || !product.IsActive || product.Stock < quantity)
            return false;

        // Get or create cart
        var cart = await _cartRepository.GetCartByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _cartRepository.AddAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        // Check if item already in cart
        var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
        if (cartItem != null)
        {
            // Update quantity
            var newQuantity = cartItem.Quantity + quantity;
            if (newQuantity > product.Stock)
                return false;

            cartItem.Quantity = newQuantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);
        }
        else
        {
            // Add new item
            cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = productId,
                Quantity = quantity
            };
            await _cartRepository.AddCartItemAsync(cartItem);
        }

        await _cartRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateCartItemQuantityAsync(int cartItemId, int quantity, string userId)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
            return false;

        // Verify ownership
        var cart = await _cartRepository.GetByIdAsync(cartItem.CartId);
        if (cart == null || cart.UserId != userId)
            return false;

        if (quantity <= 0)
        {
            await _cartRepository.RemoveCartItemAsync(cartItem);
        }
        else
        {
            // Validate stock
            var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
            if (product == null || quantity > product.Stock)
                return false;

            cartItem.Quantity = quantity;
            await _cartRepository.UpdateCartItemAsync(cartItem);
        }

        await _cartRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromCartAsync(int cartItemId, string userId)
    {
        var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
        if (cartItem == null)
            return false;

        // Verify ownership
        var cart = await _cartRepository.GetByIdAsync(cartItem.CartId);
        if (cart == null || cart.UserId != userId)
            return false;

        await _cartRepository.RemoveCartItemAsync(cartItem);
        await _cartRepository.SaveChangesAsync();
        return true;
    }

    public async Task ClearCartAsync(string userId)
    {
        var cart = await _cartRepository.GetCartWithItemsAsync(userId);
        if (cart != null && cart.CartItems.Any())
        {
            await _cartRepository.RemoveCartItemsAsync(cart.CartItems);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
