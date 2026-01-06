using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models;

public class CartItem
{
    public int Id { get; set; }
    
    public int CartId { get; set; }
    public virtual Cart? Cart { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
    
    [Required]
    [Range(1, 999)]
    public int Quantity { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.Now;
}
