using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Models;

public class Order
{
    public int Id { get; set; }
    
    public required string UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
    
    [Required]
    public required string ShippingAddress { get; set; }
    
    [StringLength(100)]
    public required string PaymentMethod { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public DateTime? ShippedDate { get; set; }
    public DateTime? DeliveredDate { get; set; }
    
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
