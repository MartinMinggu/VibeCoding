using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceApp.API.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task SendMessage(int conversationId, string message)
    {
        // Send message to specific conversation group
        await Clients.Group($"conversation_{conversationId}")
            .SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message, DateTime.UtcNow);
    }

    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
    }
}
