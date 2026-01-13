using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ECommerceApp.Web.Hubs;

namespace ECommerceApp.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly IChatService _chatService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHubContext<ChatHub> _hubContext;

    public ChatController(IChatService chatService, UserManager<ApplicationUser> userManager, IHubContext<ChatHub> hubContext)
    {
        _chatService = chatService;
        _userManager = userManager;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Inbox - List all conversations
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var conversations = await _chatService.GetUserConversationsAsync(user.Id);
        
        return View(conversations);
    }

    /// <summary>
    /// View single conversation with messages
    /// </summary>
    public async Task<IActionResult> Conversation(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var conversation = await _chatService.GetConversationAsync(id, user.Id);
        if (conversation == null) return NotFound();
        
        ViewBag.CurrentUserId = user.Id;
        return View(conversation);
    }

    /// <summary>
    /// Start new conversation with seller (from product page)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Start(string sellerId, int? productId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        // Can't chat with yourself
        if (user.Id == sellerId)
        {
            TempData["Error"] = "You cannot chat with yourself.";
            return RedirectToAction("Index", "Product");
        }

        var conversation = await _chatService.StartOrGetConversationAsync(user.Id, sellerId, productId);
        
        return RedirectToAction("Conversation", new { id = conversation.Id });
    }

    /// <summary>
    /// Send message in conversation
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(int conversationId, string content)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        if (string.IsNullOrWhiteSpace(content))
        {
            return RedirectToAction("Conversation", new { id = conversationId });
        }

        var message = await _chatService.SendMessageAsync(conversationId, user.Id, content.Trim());

        // Broadcast to SignalR group
        await _hubContext.Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", 
            user.FirstName + " " + user.LastName, 
            content, 
            message.SentAt.ToString("HH:mm"),
            user.Id // Send senderId to identify own messages
        );
        
        return RedirectToAction("Conversation", new { id = conversationId });
    }

    /// <summary>
    /// Get unread count for header badge (AJAX)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Json(new { count = 0 });

        var count = await _chatService.GetUnreadCountAsync(userId);
        return Json(new { count });
    }
}
