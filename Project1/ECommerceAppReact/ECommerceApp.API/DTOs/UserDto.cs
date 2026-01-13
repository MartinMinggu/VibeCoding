using System.Collections.Generic;

namespace ECommerceApp.API.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsSeller { get; set; }
    public List<string> Roles { get; set; } = new();
}
