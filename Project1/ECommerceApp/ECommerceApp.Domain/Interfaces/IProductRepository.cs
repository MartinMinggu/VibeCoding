using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Product entity with specific query methods
/// </summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetProductsBySellerAsync(string sellerId);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    Task<Product?> GetProductWithDetailsAsync(int id);
    Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count, string? sellerId = null);
    Task<IEnumerable<Product>> GetFilteredProductsAsync(string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, string? sortBy);
    Task DeleteProductImageAsync(ProductImage image);
    Task<ProductImage?> GetProductImageByIdAsync(int id);
}
