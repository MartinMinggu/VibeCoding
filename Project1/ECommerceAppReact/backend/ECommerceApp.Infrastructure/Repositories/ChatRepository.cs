using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly ApplicationDbContext _context;

    public ChatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Conversations
    public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(string userId)
    {
        return await _context.Conversations
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .Include(c => c.Product)
            .Include(c => c.Messages.OrderByDescending(m => m.SentAt).Take(1))
            .Where(c => c.BuyerId == userId || c.SellerId == userId)
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();
    }

    public async Task<Conversation?> GetConversationAsync(int conversationId)
    {
        return await _context.Conversations
            .Include(c => c.Buyer)
            .Include(c => c.Seller)
            .Include(c => c.Product)
            .Include(c => c.Messages.OrderBy(m => m.SentAt))
            .FirstOrDefaultAsync(c => c.Id == conversationId);
    }

    public async Task<Conversation?> GetExistingConversationAsync(string buyerId, string sellerId, int? productId)
    {
        return await _context.Conversations
            .FirstOrDefaultAsync(c => 
                c.BuyerId == buyerId && 
                c.SellerId == sellerId && 
                c.ProductId == productId);
    }

    public async Task<Conversation> CreateConversationAsync(Conversation conversation)
    {
        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();
        return conversation;
    }

    // Messages
    public async Task<IEnumerable<Message>> GetConversationMessagesAsync(int conversationId)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    public async Task<Message> SendMessageAsync(Message message)
    {
        _context.Messages.Add(message);
        
        // Update conversation's LastMessageAt
        var conversation = await _context.Conversations.FindAsync(message.ConversationId);
        if (conversation != null)
        {
            conversation.LastMessageAt = message.SentAt;
        }
        
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task MarkMessagesAsReadAsync(int conversationId, string userId)
    {
        var unreadMessages = await _context.Messages
            .Where(m => m.ConversationId == conversationId && 
                       m.SenderId != userId && 
                       !m.IsRead)
            .ToListAsync();

        foreach (var message in unreadMessages)
        {
            message.IsRead = true;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _context.Messages
            .Include(m => m.Conversation)
            .Where(m => (m.Conversation!.BuyerId == userId || m.Conversation.SellerId == userId) &&
                       m.SenderId != userId &&
                       !m.IsRead)
            .CountAsync();
    }
}
