using System.ComponentModel.DataAnnotations;

namespace ECommerceApp.Domain.Entities;

/// <summary>
/// Represents an announcement for CMS (News, Promo, Event)
/// </summary>
public class Announcement
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    public string? ImageUrl { get; set; }

    public AnnouncementType Type { get; set; } = AnnouncementType.News;

    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public required string CreatedById { get; set; }

    public virtual ApplicationUser? CreatedBy { get; set; }
}

/// <summary>
/// Type of announcement content
/// </summary>
public enum AnnouncementType
{
    News = 0,
    Promo = 1,
    Event = 2
}
