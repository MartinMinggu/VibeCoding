using ECommerceApp.Services;
using ECommerceApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductService productService, ICategoryRepository categoryRepository)
    {
        _productService = productService;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index(int? categoryId, string search)
    {
        var products = await _productService.GetAllProductsAsync(categoryId, search);
        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        ViewBag.SelectedCategory = categoryId;

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductDetailsAsync(id);

        if (product == null)
            return NotFound();

        return View(product);
    }
}
