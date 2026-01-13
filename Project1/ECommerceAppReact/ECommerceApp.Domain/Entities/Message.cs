using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

/// <summary>
/// Represents a single message in a conversation
/// </summary>
public class Message
{
    public int Id { get; set; }
    
    public int ConversationId { get; set; }
    public virtual Conversation? Conversation { get; set; }
    
    [Required]
    public required string SenderId { get; set; }
    public virtual ApplicationUser? Sender { get; set; }
    
    [Required]
    [StringLength(2000)]
    public required string Content { get; set; }
    
    public DateTime SentAt { get; set; } = DateTime.Now;
    
    public bool IsRead { get; set; } = false;
}
