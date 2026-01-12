using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Announcement entity
/// </summary>
public interface IAnnouncementRepository : IRepository<Announcement>
{
    /// <summary>
    /// Get all active announcements that are currently within their scheduled dates
    /// </summary>
    Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync();

    /// <summary>
    /// Get announcements by type
    /// </summary>
    Task<IEnumerable<Announcement>> GetByTypeAsync(AnnouncementType type);

    /// <summary>
    /// Get all announcements including inactive ones (for CMS management)
    /// </summary>
    Task<IEnumerable<Announcement>> GetAllWithCreatorAsync();
}
