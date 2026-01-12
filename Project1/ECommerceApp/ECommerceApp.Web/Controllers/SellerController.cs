using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Hosting;

namespace ECommerceApp.Controllers;

[Authorize]
public class SellerController : Controller
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SellerController(
        IProductService productService,
        IOrderService orderService,
        ICategoryRepository categoryRepository,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment webHostEnvironment)
    {
        _productService = productService;
        _orderService = orderService;
        _categoryRepository = categoryRepository;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
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
    public async Task<IActionResult> EditProduct(int id, UpdateProductDto dto, List<IFormFile> galleryImages)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        var success = await _productService.UpdateProductAsync(id, dto, user.Id);
        if (!success)
            return NotFound();

        // Handle Gallery Images
        if (galleryImages != null && galleryImages.Count > 0)
        {
            var uploadedUrls = new List<string>();
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in galleryImages)
            {
                if (file.Length > 0)
                {
                    // Generate unique filename
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    uploadedUrls.Add($"/images/products/{fileName}");
                }
            }

            if (uploadedUrls.Count > 0)
            {
                await _productService.AddProductGalleryImagesAsync(id, uploadedUrls);
            }
        }

        TempData["Success"] = "Product updated successfully!";
        return RedirectToAction("Products");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteGalleryImage(int imageId, int productId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || !user.IsSeller)
            return RedirectToAction("Index", "Home");

        // We use productId only for redirection, security is handled in service
        var success = await _productService.RemoveProductGalleryImageAsync(imageId, user.Id);
        
        if (success)
            TempData["Success"] = "Image removed.";
        else
            TempData["Error"] = "Could not remove image.";

        return RedirectToAction("EditProduct", new { id = productId });
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
