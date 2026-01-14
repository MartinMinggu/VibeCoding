using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? search,
        [FromQuery] int? categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
    {
        try
        {
            var filter = new ProductFilterDto
            {
                Search = search,
                CategoryId = categoryId
            };

            var allProducts = (await _productService.GetFilteredProductsAsync(filter)).ToList();
            var totalCount = allProducts.Count;

            var products = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                products,
                currentPage = page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                hasMore = (page * pageSize) < totalCount
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving products", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await _productService.GetProductDetailsAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving product", error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var productDto = new CreateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId
            };

            var sellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sellerId))
            {
                return Unauthorized(new { message = "User ID not found in token" });
            }

            var createdProduct = await _productService.CreateProductAsync(productDto, sellerId);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error creating product", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");

            var product = await _productService.GetProductDetailsAsync(id, includeInactive: true);
            if (product == null) return NotFound(new { message = "Product not found" });

            // Allow if user is owner OR user is Admin
            if (product.SellerId != userId && !isAdmin)
            {
                return Forbid();
            }

            var productDto = new UpdateProductDto
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                IsActive = true // Ensure active on update
            };

            // Pass the original sellerId to service to satisfy the ownership check logic
            // (Service thinks the owner is performing the action)
            await _productService.UpdateProductAsync(id, productDto, product.SellerId);
            
            return Ok(new { message = "Product updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating product", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin") || User.IsInRole("SuperAdmin");

            var product = await _productService.GetProductDetailsAsync(id, includeInactive: true);
            if (product == null) return NotFound(new { message = "Product not found" });

            if (product.SellerId != userId && !isAdmin)
            {
                return Forbid();
            }

            // Pass original sellerId to service
            await _productService.DeleteProductAsync(id, product.SellerId);
            
            return Ok(new { message = "Product deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting product", error = ex.Message });
        }
    }
}

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
