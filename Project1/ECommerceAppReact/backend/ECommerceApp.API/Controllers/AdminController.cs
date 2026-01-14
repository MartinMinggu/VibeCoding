using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ECommerceApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    isSeller = user.IsSeller,
                    roles = roles
                });
            }

            return Ok(userList);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error fetching users", error = ex.Message });
        }
    }

    [HttpGet("admins")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAdmins()
    {
        try
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");

            var allAdmins = admins.Concat(superAdmins).Distinct();

            var result = new List<object>();
            foreach (var admin in allAdmins)
            {
                var roles = await _userManager.GetRolesAsync(admin);
                result.Add(new
                {
                    id = admin.Id,
                    email = admin.Email,
                    firstName = admin.FirstName,
                    lastName = admin.LastName,
                    roles = roles
                });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error fetching admins", error = ex.Message });
        }
    }

    [HttpPost("promote/{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Check if already Admin
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return BadRequest(new { message = "User is already an Admin" });

            // Ensure Admin role exists
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to promote user", errors = result.Errors });

            return Ok(new { message = $"{user.Email} has been promoted to Admin" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error promoting user", error = ex.Message });
        }
    }

    [HttpPost("demote/{userId}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> DemoteFromAdmin(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // Prevent demoting SuperAdmin
            if (await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                return BadRequest(new { message = "Cannot demote SuperAdmin" });

            // Check if user is Admin
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
                return BadRequest(new { message = "User is not an Admin" });

            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
            if (!result.Succeeded)
                return BadRequest(new { message = "Failed to demote user", errors = result.Errors });

            return Ok(new { message = $"{user.Email} has been demoted from Admin" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error demoting user", error = ex.Message });
        }
    }
}
