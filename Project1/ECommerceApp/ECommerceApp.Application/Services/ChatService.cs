using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId)
    {
        return await _chatRepository.GetUserConversationsAsync(userId);
    }

    public async Task<Conversation?> GetConversationAsync(int conversationId, string userId)
    {
        var conversation = await _chatRepository.GetConversationAsync(conversationId);
        
        // Verify user has access to this conversation
        if (conversation != null && 
            conversation.BuyerId != userId && 
            conversation.SellerId != userId)
        {
            return null;
        }

        // Mark messages as read when viewing conversation
        if (conversation != null)
        {
            await _chatRepository.MarkMessagesAsReadAsync(conversationId, userId);
        }

        return conversation;
    }

    public async Task<Conversation> StartOrGetConversationAsync(string buyerId, string sellerId, int? productId)
    {
        // Check if conversation already exists
        var existing = await _chatRepository.GetExistingConversationAsync(buyerId, sellerId, productId);
        if (existing != null)
        {
            return existing;
        }

        // Create new conversation
        var conversation = new Conversation
        {
            BuyerId = buyerId,
            SellerId = sellerId,
            ProductId = productId,
            CreatedAt = DateTime.Now,
            LastMessageAt = DateTime.Now
        };

        return await _chatRepository.CreateConversationAsync(conversation);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(int conversationId, string userId)
    {
        // Verify access first
        var conversation = await _chatRepository.GetConversationAsync(conversationId);
        if (conversation == null || 
            (conversation.BuyerId != userId && conversation.SellerId != userId))
        {
            return Enumerable.Empty<Message>();
        }

        // Mark as read
        await _chatRepository.MarkMessagesAsReadAsync(conversationId, userId);

        return await _chatRepository.GetConversationMessagesAsync(conversationId);
    }

    public async Task<Message> SendMessageAsync(int conversationId, string senderId, string content)
    {
        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.Now,
            IsRead = false
        };

        return await _chatRepository.SendMessageAsync(message);
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _chatRepository.GetUnreadCountAsync(userId);
    }
}
