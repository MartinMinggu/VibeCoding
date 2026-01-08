using ECommerceApp.Application.DTOs;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service interface for Product business logic
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? categoryId = null, string? search = null);
    Task<ProductDetailsDto?> GetProductDetailsAsync(int id);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, string sellerId);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto dto, string sellerId);
    Task<bool> DeleteProductAsync(int id, string sellerId);
    Task<IEnumerable<ProductDto>> GetSellerProductsAsync(string sellerId);
    Task<IEnumerable<ProductDto>> GetTopSellingProductsAsync(int count, string? sellerId = null);
    Task<int> GetProductCountBySellerAsync(string sellerId);
}
