using ECommerceApp.Models;

namespace ECommerceApp.Repositories;

/// <summary>
/// Repository interface for Category entity
/// </summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetActiveCategoriesAsync();
}
