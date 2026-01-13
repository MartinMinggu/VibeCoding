using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public required string UserId { get; set; }
    public virtual ApplicationUser? User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
