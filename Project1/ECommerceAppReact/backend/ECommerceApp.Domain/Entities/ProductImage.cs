using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

/// <summary>
/// Represents an image for a product (supports multiple images per product)
/// </summary>
public class ProductImage
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    [Required]
    [StringLength(500)]
    public required string ImageUrl { get; set; }
    
    /// <summary>
    /// Whether this is the main/primary image shown in listings
    /// </summary>
    public bool IsPrimary { get; set; } = false;
    
    /// <summary>
    /// Order in which images appear in gallery (lower = first)
    /// </summary>
    public int DisplayOrder { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public virtual Product? Product { get; set; }
}
