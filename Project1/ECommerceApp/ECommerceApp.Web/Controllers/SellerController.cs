using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Controllers;

[Authorize]
public class SellerController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public SellerController(
        IProductService productService,
        IOrderService orderService,
        ICategoryRepository categoryRepository,
        UserManager<ApplicationUser> userManager)
    {
        _productService = productService;
        _orderService = orderService;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        ViewBag.ProductCount = await _productService.GetProductCountBySellerAsync(user.Id);
        ViewBag.TotalOrders = await _orderService.GetOrderCountBySellerAsync(user.Id);

        var products = await _productService.GetSellerProductsAsync(user.Id);
        var recentProducts = products.OrderByDescending(p => p.CreatedAt).Take(5);

        return View(recentProducts);
    }

    public async Task<IActionResult> Products()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var products = await _productService.GetSellerProductsAsync(user.Id);
        return View(products);
    }

    public async Task<IActionResult> CreateProduct()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        await _productService.CreateProductAsync(dto, user.Id);
        TempData["Success"] = "Product created successfully!";
        return RedirectToAction("Products");
    }

    public async Task<IActionResult> EditProduct(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var product = await _productService.GetProductDetailsAsync(id);
        if (product == null || product.SellerId != user.Id)
            return NotFound();

        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> EditProduct(int id, UpdateProductDto dto)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var success = await _productService.UpdateProductAsync(id, dto, user.Id);
        if (!success)
            return NotFound();

        TempData["Success"] = "Product updated successfully!";
        return RedirectToAction("Products");
    }

    public async Task<IActionResult> Orders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var orders = await _orderService.GetSellerOrdersAsync(user.Id);
        return View(orders);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var success = await _orderService.UpdateOrderStatusAsync(orderId, status, user.Id);
        if (!success)
            return NotFound();

        TempData["Success"] = $"Order status updated to {status}";
        return RedirectToAction("Orders");
    }
}
