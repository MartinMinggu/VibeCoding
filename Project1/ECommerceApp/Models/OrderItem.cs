using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models;

public class OrderItem
{
    public int Id { get; set; }
    
    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }
    
    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
    
    [Required]
    [Range(1, 999)]
    public int Quantity { get; set; }
    
    [Required]
    public decimal UnitPrice { get; set; }
}
