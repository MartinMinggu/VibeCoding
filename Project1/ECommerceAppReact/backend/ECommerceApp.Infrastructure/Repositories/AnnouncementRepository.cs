using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Announcement entity
/// </summary>
public class AnnouncementRepository : IAnnouncementRepository
{
    private readonly ApplicationDbContext _context;

    public AnnouncementRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        return await _context.Announcements
            .Include(a => a.CreatedBy)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Announcement>> GetAllAsync()
    {
        return await _context.Announcements
            .Include(a => a.CreatedBy)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<Announcement> AddAsync(Announcement entity)
    {
        await _context.Announcements.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Announcement entity)
    {
        _context.Announcements.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Announcement entity)
    {
        _context.Announcements.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Announcements.AnyAsync(a => a.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Announcement>> GetActiveAnnouncementsAsync()
    {
        var now = DateTime.Now;
        return await _context.Announcements
            .Include(a => a.CreatedBy)
            .Where(a => a.IsActive 
                && (a.StartDate == null || a.StartDate <= now)
                && (a.EndDate == null || a.EndDate >= now))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Announcement>> GetByTypeAsync(AnnouncementType type)
    {
        return await _context.Announcements
            .Include(a => a.CreatedBy)
            .Where(a => a.Type == type && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Announcement>> GetAllWithCreatorAsync()
    {
        return await _context.Announcements
            .Include(a => a.CreatedBy)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }
}
