using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Domain.Interfaces;

namespace ECommerceApp.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Product entity
/// </summary>
public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .ToListAsync();
    }

    public async Task<Product> AddAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        return entity;
    }

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(Product entity)
    {
        _context.Products.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Where(p => p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsBySellerAsync(string sellerId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.SellerId == sellerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Seller)
            .Where(p => p.IsActive && 
                   (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            //.Include(p => p.Seller) // Relaxed for mock data
            .Include(p => p.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task DeleteProductImageAsync(ProductImage image)
    {
        _context.Set<ProductImage>().Remove(image);
        await Task.CompletedTask;
    }

    public async Task<ProductImage?> GetProductImageByIdAsync(int id)
    {
        return await _context.Set<ProductImage>()
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Product>> GetTopSellingProductsAsync(int count, string? sellerId = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(sellerId))
        {
            query = query.Where(p => p.SellerId == sellerId);
        }

        return await query
            .OrderByDescending(p => p.OrderItems.Sum(oi => oi.Quantity))
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetFilteredProductsAsync(
        string? search, int? categoryId, decimal? minPrice, decimal? maxPrice, string? sortBy)
    {
        var query = _context.Products
            .Include(p => p.Category)
            //.Include(p => p.Seller) // Temporarily commented out to show products with invalid SellerId
            .Where(p => p.IsActive)
            .AsSplitQuery()
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchLower = search.ToLower();
            query = query.Where(p => 
                p.Name.ToLower().Contains(searchLower) || 
                p.Description.ToLower().Contains(searchLower));
        }

        // Apply category filter
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        // Apply price range filter
        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // Apply sorting
        query = sortBy?.ToLower() switch
        {
            "price_asc" => query.OrderBy(p => p.Price),
            "price_desc" => query.OrderByDescending(p => p.Price),
            "name_asc" => query.OrderBy(p => p.Name),
            "name_desc" => query.OrderByDescending(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.CreatedAt) // Default: newest first
        };

        return await query.ToListAsync();
    }
}
