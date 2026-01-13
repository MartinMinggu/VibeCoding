using Microsoft.AspNetCore.Mvc;
using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid email or password" });

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = "mock-jwt-token-" + Guid.NewGuid(),
                refreshToken = Guid.NewGuid().ToString(),
                expiration = DateTime.UtcNow.AddHours(1),
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    isSeller = user.IsSeller,
                    roles
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Login error", error = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsSeller = request.RegisterAsSeller,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            var role = request.RegisterAsSeller ? "Seller" : "Customer";
            await _userManager.AddToRoleAsync(user, role);

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                token = "mock-jwt-token-" + Guid.NewGuid(),
                refreshToken = Guid.NewGuid().ToString(),
                expiration = DateTime.UtcNow.AddHours(1),
                user = new
                {
                    id = user.Id,
                    email = user.Email,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    isSeller = user.IsSeller,
                    roles
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Registration error", error = ex.Message });
        }
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool RegisterAsSeller { get; set; }
}
