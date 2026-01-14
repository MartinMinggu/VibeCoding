using ECommerceApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var cart = await _cartService.GetUserCartAsync(userId);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var success = await _cartService.AddToCartAsync(userId, productId, quantity);
        
        if (!success)
        {
            TempData["Error"] = "Product not available or insufficient stock";
            return RedirectToAction("Details", "Product", new { id = productId });
        }

        TempData["Success"] = "Product added to cart!";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int id, int quantity)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var success = await _cartService.UpdateCartItemQuantityAsync(id, quantity, userId);
        
        if (!success)
            TempData["Error"] = "Unable to update quantity";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        await _cartService.RemoveFromCartAsync(id, userId);
        return RedirectToAction("Index");
    }
}
