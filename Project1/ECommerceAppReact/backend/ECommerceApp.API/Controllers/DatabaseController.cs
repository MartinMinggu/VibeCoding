using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DatabaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("clear")]
    public async Task<IActionResult> ClearDatabase()
    {
        try
        {
            // Execute in a transaction to ensure atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Delete in dependency order (Child -> Parent)
                
                // 1. Level 3 (Grandchildren)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"ProductGalleryImages\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"OrderItems\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"CartItems\"");
                
                // 2. Level 2 (Children)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"Orders\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"Carts\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"Products\"");
                
                // 3. Level 1 (Parents)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"Categories\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserRoles\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserClaims\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserLogins\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUserTokens\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetRoleClaims\"");
                
                // 4. Level 0 (Roots)
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetUsers\"");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM \"AspNetRoles\"");

                await transaction.CommitAsync();

                return Ok(new { message = "Database cleared successfully. All data has been deleted." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error clearing database", error = ex.Message });
        }
    }
}
