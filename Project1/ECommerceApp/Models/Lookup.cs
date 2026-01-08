using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models;

public class Lookup
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public required string Key { get; set; }
    
    [Required]
    public required string Value { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(50)]
    public string? Category { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public DateTime? UpdatedAt { get; set; }
}
