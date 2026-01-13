using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Chat operations
/// </summary>
public interface IChatRepository
{
    // Conversations
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
    Task<Conversation?> GetConversationAsync(int conversationId);
    Task<Conversation?> GetExistingConversationAsync(string buyerId, string sellerId, int? productId);
    Task<Conversation> CreateConversationAsync(Conversation conversation);
    
    // Messages
    Task<IEnumerable<Message>> GetConversationMessagesAsync(int conversationId);
    Task<Message> SendMessageAsync(Message message);
    Task MarkMessagesAsReadAsync(int conversationId, string userId);
    Task<int> GetUnreadCountAsync(string userId);
}
