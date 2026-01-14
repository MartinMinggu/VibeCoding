using ECommerceApp.Domain.Entities;

namespace ECommerceApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Category entity
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}
