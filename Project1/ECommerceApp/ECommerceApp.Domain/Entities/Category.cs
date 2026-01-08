using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
