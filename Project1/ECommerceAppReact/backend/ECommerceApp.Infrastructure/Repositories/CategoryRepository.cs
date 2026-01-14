using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Category entity
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> AddAsync(Category entity)
    {
        await _context.Categories.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Category entity)
    {
        _context.Categories.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Category entity)
    {
        _context.Categories.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}
