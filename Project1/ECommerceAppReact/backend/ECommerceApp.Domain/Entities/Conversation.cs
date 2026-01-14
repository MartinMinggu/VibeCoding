using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

/// <summary>
/// Represents a chat conversation between a buyer and seller
/// </summary>
public class Conversation
{
    public int Id { get; set; }
    
    [Required]
    public required string BuyerId { get; set; }
    public virtual ApplicationUser? Buyer { get; set; }
    
    [Required]
    public required string SellerId { get; set; }
    public virtual ApplicationUser? Seller { get; set; }
    
    /// <summary>
    /// Optional: The product this conversation is about
    /// </summary>
    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastMessageAt { get; set; } = DateTime.Now;
    
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
