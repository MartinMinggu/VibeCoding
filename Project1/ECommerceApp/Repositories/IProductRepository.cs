using ECommerceApp.Models;

namespace ECommerceApp.Repositories;

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
}
