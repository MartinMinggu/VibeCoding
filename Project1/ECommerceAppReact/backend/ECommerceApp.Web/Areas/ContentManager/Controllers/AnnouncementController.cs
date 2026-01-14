using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Application.DTOs;
using ECommerceApp.Application.Services;
using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Web.Areas.ContentManager.Controllers;

[Area("ContentManager")]
[Authorize(Roles = "ContentManager,Admin")]
public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AnnouncementController(
        IAnnouncementService announcementService, 
        UserManager<ApplicationUser> userManager)
    {
        _announcementService = announcementService;
        _userManager = userManager;
    }

    // GET: ContentManager/Announcement
    public async Task<IActionResult> Index()
    {
        var announcements = await _announcementService.GetAllAnnouncementsAsync();
        return View(announcements);
    }

    // GET: ContentManager/Announcement/Create
    public IActionResult Create()
    {
        return View(new CreateAnnouncementDto());
    }

    // POST: ContentManager/Announcement/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAnnouncementDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        await _announcementService.CreateAnnouncementAsync(dto, user.Id);
        TempData["Success"] = "Announcement created successfully!";
        return RedirectToAction(nameof(Index));
    }

    // GET: ContentManager/Announcement/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
        if (announcement == null)
            return NotFound();

        var dto = new UpdateAnnouncementDto
        {
            Title = announcement.Title,
            Content = announcement.Content,
            ImageUrl = announcement.ImageUrl,
            Type = announcement.Type,
            StartDate = announcement.StartDate,
            EndDate = announcement.EndDate,
            IsActive = announcement.IsActive
        };

        ViewBag.AnnouncementId = id;
        return View(dto);
    }

    // POST: ContentManager/Announcement/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateAnnouncementDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AnnouncementId = id;
            return View(dto);
        }

        var success = await _announcementService.UpdateAnnouncementAsync(id, dto);
        if (!success)
            return NotFound();

        TempData["Success"] = "Announcement updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    // GET: ContentManager/Announcement/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var announcement = await _announcementService.GetAnnouncementByIdAsync(id);
        if (announcement == null)
            return NotFound();

        return View(announcement);
    }

    // POST: ContentManager/Announcement/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _announcementService.DeleteAnnouncementAsync(id);
        if (!success)
            return NotFound();

        TempData["Success"] = "Announcement deleted successfully!";
        return RedirectToAction(nameof(Index));
    }
}
