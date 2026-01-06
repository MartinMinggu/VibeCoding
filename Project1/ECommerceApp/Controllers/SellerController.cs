using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers;

[Authorize]
public class SellerController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SellerController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var userId = user.Id;
        ViewBag.ProductCount = await _context.Products.CountAsync(p => p.SellerId == userId);
        ViewBag.TotalOrders = await _context.OrderItems
            .Include(oi => oi.Product)
            .Where(oi => oi.Product != null && oi.Product.SellerId == userId)
            .Select(oi => oi.OrderId)
            .Distinct()
            .CountAsync();

        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.SellerId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> Products()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.SellerId == user.Id)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        product.SellerId = user.Id;
        product.CreatedAt = DateTime.Now;
        product.IsActive = true;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        TempData["Success"] = "Product created successfully!";
        return RedirectToAction("Products");
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.SellerId == user.Id);

        if (product == null)
            return NotFound();

        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(Product product)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id && p.SellerId == user.Id);

        if (existingProduct == null)
            return NotFound();

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.ImageUrl = product.ImageUrl;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.IsActive = product.IsActive;

        await _context.SaveChangesAsync();
        TempData["Success"] = "Product updated successfully!";
        return RedirectToAction("Products");
    }

    public async Task<IActionResult> Orders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var orders = await _context.OrderItems
            .Include(oi => oi.Order)
            .ThenInclude(o => o != null ? o.User : null)
            .Include(oi => oi.Product)
            .Where(oi => oi.Product != null && oi.Product.SellerId == user.Id)
            .OrderByDescending(oi => oi.Order != null ? oi.Order.OrderDate : DateTime.MinValue)
            .ToListAsync();

        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            return NotFound();

        // Verify seller owns at least one product in this order
        var hasSellerProduct = order.OrderItems.Any(oi => oi.Product != null && oi.Product.SellerId == user.Id);
        if (!hasSellerProduct)
            return Forbid();

        order.Status = status;
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Order status updated to {status}";
        return RedirectToAction("Orders");
    }
}
