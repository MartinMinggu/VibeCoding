using ECommerceApp.Application.DTOs;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Service interface for Announcement business logic
/// </summary>
public interface IAnnouncementService
{
    /// <summary>
    /// Get all announcements for CMS management
    /// </summary>
    Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync();

    /// <summary>
    /// Get active announcements for display on home page
    /// </summary>
    Task<IEnumerable<AnnouncementDto>> GetActiveAnnouncementsAsync();

    /// <summary>
    /// Get announcement by ID
    /// </summary>
    Task<AnnouncementDto?> GetAnnouncementByIdAsync(int id);

    /// <summary>
    /// Create a new announcement
    /// </summary>
    Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto dto, string createdById);

    /// <summary>
    /// Update an existing announcement
    /// </summary>
    Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto dto);

    /// <summary>
    /// Delete an announcement
    /// </summary>
    Task<bool> DeleteAnnouncementAsync(int id);
}
