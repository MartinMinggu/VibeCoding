using ECommerceApp.Application.DTOs;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service implementation for Product business logic
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? categoryId = null, string? search = null)
    {
        IEnumerable<Product> products;

        if (categoryId.HasValue)
        {
            products = await _productRepository.GetProductsByCategoryAsync(categoryId.Value);
        }
        else if (!string.IsNullOrEmpty(search))
        {
            products = await _productRepository.SearchProductsAsync(search);
        }
        else
        {
            products = await _productRepository.GetActiveProductsAsync();
        }

        return products.Select(MapToDto);
    }

    public async Task<ProductDetailsDto?> GetProductDetailsAsync(int id)
    {
        var product = await _productRepository.GetProductWithDetailsAsync(id);
        if (product == null || !product.IsActive)
            return null;

        return new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty,
            SellerId = product.SellerId,
            SellerName = product.Seller != null ? $"{product.Seller.FirstName} {product.Seller.LastName}" : string.Empty,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto dto, string sellerId)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            SellerId = sellerId,
            CreatedAt = DateTime.Now,
            IsActive = true
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

        // Reload with details
        var createdProduct = await _productRepository.GetProductWithDetailsAsync(product.Id);
        return MapToDto(createdProduct!);
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto, string sellerId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null || product.SellerId != sellerId)
            return false;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;
        product.IsActive = dto.IsActive;

        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteProductAsync(int id, string sellerId)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null || product.SellerId != sellerId)
            return false;

        // Soft delete
        product.IsActive = false;
        await _productRepository.UpdateAsync(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ProductDto>> GetSellerProductsAsync(string sellerId)
    {
        var products = await _productRepository.GetProductsBySellerAsync(sellerId);
        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetTopSellingProductsAsync(int count, string? sellerId = null)
    {
        var products = await _productRepository.GetTopSellingProductsAsync(count, sellerId);
        return products.Select(MapToDto);
    }

    public async Task<int> GetProductCountBySellerAsync(string sellerId)
    {
        var products = await _productRepository.GetProductsBySellerAsync(sellerId);
        return products.Count();
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryName = product.Category?.Name ?? string.Empty,
            SellerName = product.Seller != null ? $"{product.Seller.FirstName} {product.Seller.LastName}" : string.Empty,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}
