using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Interfaces;
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

    public async Task<IActionResult> Index(
        string? search, 
        int? categoryId, 
        decimal? minPrice, 
        decimal? maxPrice, 
        string? sortBy)
    {
        var filter = new ProductFilterDto
        {
            Search = search,
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SortBy = sortBy
        };
        
        var allProducts = await _productService.GetFilteredProductsAsync(filter);
        var products = allProducts.Take(10).ToList(); // Initial load: 20 products only
        
        ViewBag.Categories = await _categoryRepository.GetAllAsync();
        ViewBag.SelectedCategory = categoryId;
        ViewBag.Search = search;
        ViewBag.MinPrice = minPrice;
        ViewBag.MaxPrice = maxPrice;
        ViewBag.SortBy = sortBy;
        ViewBag.TotalCount = allProducts.Count();
        ViewBag.HasMore = allProducts.Count() > 10;

        return View(products);
    }

    /// <summary>
    /// API endpoint for infinite scroll - returns paginated products as JSON
    /// </summary>
    [HttpGet]
    [Route("Product/GetProductsApi")]
    public async Task<IActionResult> GetProductsApi(
        int page = 1,
        int pageSize = 10,
        string? search = null,
        int? categoryId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null)
    {
        var filter = new ProductFilterDto
        {
            Search = search,
            CategoryId = categoryId,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            SortBy = sortBy
        };

        var allProducts = (await _productService.GetFilteredProductsAsync(filter)).ToList();
        var totalCount = allProducts.Count;
        
        // Pagination
        var products = allProducts
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new
        {
            products = products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                stock = p.Stock,
                imageUrl = p.ImageUrl,
                categoryName = p.CategoryName,
                sellerName = p.SellerName
            }),
            currentPage = page,
            pageSize = pageSize,
            totalCount = totalCount,
            hasMore = (page * pageSize) < totalCount
        };

        return Json(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductDetailsAsync(id);

        if (product == null)
            return NotFound();

        // Get similar products from same category
        var similarProducts = await _productService.GetSimilarProductsAsync(id, product.CategoryId, 8);
        ViewBag.SimilarProducts = similarProducts;

        return View(product);
    }

    /// <summary>
    /// API endpoint for fetching recently viewed products by IDs
    /// </summary>
    [HttpGet]
    [Route("Product/GetRecentlyViewedApi")]
    public async Task<IActionResult> GetRecentlyViewedApi([FromQuery] List<int> ids)
    {
        if (ids == null || !ids.Any())
            return Json(new { products = new List<object>() });

        var products = await _productService.GetProductsByIdsAsync(ids);
        
        var result = products.Select(p => new
        {
            id = p.Id,
            name = p.Name,
            price = p.Price,
            imageUrl = p.ImageUrl,
            categoryName = p.CategoryName
        });

        return Json(new { products = result });
    }
}
