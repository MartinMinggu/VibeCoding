using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
            return RedirectToAction("Index", "Cart");

        var user = await _userManager.GetUserAsync(User);
        ViewBag.ShippingAddress = user.Address;

        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string shippingAddress, string paymentMethod)
    {
        var userId = _userManager.GetUserId(User);
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null || !cart.CartItems.Any())
            return RedirectToAction("Index", "Cart");

        var order = new Order
        {
            UserId = userId,
            ShippingAddress = shippingAddress,
            PaymentMethod = paymentMethod,
            TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);

        foreach (var item in cart.CartItems)
        {
            var product = await _context.Products.FindAsync(item.ProductId);
            product.Stock -= item.Quantity;
        }

        _context.CartItems.RemoveRange(cart.CartItems);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Order placed successfully!";
        return RedirectToAction("OrderConfirmation", new { id = order.Id });
    }

    public async Task<IActionResult> OrderConfirmation(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            return NotFound();

        return View(order);
    }

    public async Task<IActionResult> MyOrders()
    {
        var userId = _userManager.GetUserId(User);
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }
}
