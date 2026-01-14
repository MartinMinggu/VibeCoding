using ECommerceApp.Application.DTOs;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service interface for Product business logic
/// </summary>
public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync(int? categoryId = null, string? search = null);
    Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(ProductFilterDto filter);
    Task<bool> AddProductGalleryImagesAsync(int productId, List<string> imageUrls);
    Task<bool> RemoveProductGalleryImageAsync(int imageId, string sellerId);
    Task<ProductDetailsDto?> GetProductDetailsAsync(int id, bool includeInactive = false);
    Task<ProductDto> CreateProductAsync(CreateProductDto dto, string sellerId);
    Task<bool> UpdateProductAsync(int id, UpdateProductDto dto, string sellerId);
    Task<bool> DeleteProductAsync(int id, string sellerId);
    Task<IEnumerable<ProductDto>> GetSellerProductsAsync(string sellerId);
    Task<IEnumerable<ProductDto>> GetTopSellingProductsAsync(int count, string? sellerId = null);
    Task<int> GetProductCountBySellerAsync(string sellerId);
    Task<IEnumerable<ProductDto>> GetSimilarProductsAsync(int productId, int categoryId, int count = 8);
    Task<IEnumerable<ProductDto>> GetProductsByIdsAsync(List<int> productIds);
}
