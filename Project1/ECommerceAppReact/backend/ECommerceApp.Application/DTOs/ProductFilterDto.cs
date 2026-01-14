namespace ECommerceApp.Application.DTOs;

/// <summary>
/// DTO for product filtering and search parameters
/// </summary>
public class ProductFilterDto
{
    /// <summary>
    /// Search term to find in product name or description
    /// </summary>
    public string? Search { get; set; }
    
    /// <summary>
    /// Filter by category ID
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    /// Minimum price filter
    /// </summary>
    public decimal? MinPrice { get; set; }
    
    /// <summary>
    /// Maximum price filter
    /// </summary>
    public decimal? MaxPrice { get; set; }
    
    /// <summary>
    /// Sort option: "price_asc", "price_desc", "name_asc", "name_desc", "newest"
    /// </summary>
    public string? SortBy { get; set; }
}
