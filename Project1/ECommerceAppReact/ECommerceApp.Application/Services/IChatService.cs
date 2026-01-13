using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Application.Services;

public interface IChatService
{
    Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId);
    Task<Conversation?> GetConversationAsync(int conversationId, string userId);
    Task<Conversation> StartOrGetConversationAsync(string buyerId, string sellerId, int? productId);
    Task<IEnumerable<Message>> GetMessagesAsync(int conversationId, string userId);
    Task<Message> SendMessageAsync(int conversationId, string senderId, string content);
    Task<int> GetUnreadCountAsync(string userId);
}
