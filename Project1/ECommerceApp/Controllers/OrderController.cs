using ECommerceApp.Services;
using ECommerceApp.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Models;

namespace ECommerceApp.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
    {
        _orderService = orderService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = await _userManager.GetUserAsync(User);
        var checkout = await _orderService.GetCheckoutDetailsAsync(userId, user?.Address ?? string.Empty);

        if (checkout == null)
            return RedirectToAction("Index", "Cart");

        return View(checkout);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string shippingAddress, string paymentMethod)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var dto = new PlaceOrderDto
        {
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod
        };

        var orderId = await _orderService.PlaceOrderAsync(userId, dto);

        if (orderId == null)
        {
            TempData["Error"] = "Unable to place order. Please check your cart.";
            return RedirectToAction("Index", "Cart");
        }

        TempData["Success"] = "Order placed successfully!";
        return RedirectToAction("OrderConfirmation", new { id = orderId });
    }

    public async Task<IActionResult> OrderConfirmation(int id)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var order = await _orderService.GetOrderDetailsAsync(id, userId);

        if (order == null)
            return NotFound();

        return View(order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var orders = await _orderService.GetUserOrdersAsync(userId);
        return View(orders);
    }
}
