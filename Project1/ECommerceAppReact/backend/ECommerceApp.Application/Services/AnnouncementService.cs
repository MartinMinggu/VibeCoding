using ECommerceApp.Application.DTOs;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Application.Services;

/// <summary>
/// Implementation of announcement service
/// </summary>
public class AnnouncementService : IAnnouncementService
{
    private readonly IAnnouncementRepository _announcementRepository;

    public AnnouncementService(IAnnouncementRepository announcementRepository)
    {
        _announcementRepository = announcementRepository;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync()
    {
        var announcements = await _announcementRepository.GetAllWithCreatorAsync();
        return announcements.Select(MapToDto);
    }

    public async Task<IEnumerable<AnnouncementDto>> GetActiveAnnouncementsAsync()
    {
        var announcements = await _announcementRepository.GetActiveAnnouncementsAsync();
        return announcements.Select(MapToDto);
    }

    public async Task<AnnouncementDto?> GetAnnouncementByIdAsync(int id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);
        return announcement == null ? null : MapToDto(announcement);
    }

    public async Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto dto, string createdById)
    {
        var announcement = new Announcement
        {
            Title = dto.Title,
            Content = dto.Content,
            ImageUrl = dto.ImageUrl,
            Type = dto.Type,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive,
            CreatedById = createdById,
            CreatedAt = DateTime.Now
        };

        var created = await _announcementRepository.AddAsync(announcement);
        await _announcementRepository.SaveChangesAsync();
        return MapToDto(created);
    }

    public async Task<bool> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto dto)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);
        if (announcement == null)
            return false;

        announcement.Title = dto.Title;
        announcement.Content = dto.Content;
        announcement.ImageUrl = dto.ImageUrl;
        announcement.Type = dto.Type;
        announcement.StartDate = dto.StartDate;
        announcement.EndDate = dto.EndDate;
        announcement.IsActive = dto.IsActive;
        announcement.UpdatedAt = DateTime.Now;

        await _announcementRepository.UpdateAsync(announcement);
        await _announcementRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAnnouncementAsync(int id)
    {
        var announcement = await _announcementRepository.GetByIdAsync(id);
        if (announcement == null)
            return false;

        await _announcementRepository.DeleteAsync(announcement);
        await _announcementRepository.SaveChangesAsync();
        return true;
    }

    private static AnnouncementDto MapToDto(Announcement announcement)
    {
        return new AnnouncementDto
        {
            Id = announcement.Id,
            Title = announcement.Title,
            Content = announcement.Content,
            ImageUrl = announcement.ImageUrl,
            Type = announcement.Type,
            StartDate = announcement.StartDate,
            EndDate = announcement.EndDate,
            IsActive = announcement.IsActive,
            CreatedAt = announcement.CreatedAt,
            CreatedByName = announcement.CreatedBy != null 
                ? $"{announcement.CreatedBy.FirstName} {announcement.CreatedBy.LastName}" 
                : null
        };
    }
}
