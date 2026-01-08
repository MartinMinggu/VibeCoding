using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ECommerceApp.Tests;

public class LoginTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LoginTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add an in-memory database for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });
            });
        });
    }

    [Fact]
    public async Task Get_Login_ReturnsSuccessAndCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Identity/Account/Login");

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType!.ToString());
    }

    [Fact]
    public async Task Post_Login_WithValidCredentials_RedirectsToIndex()
    {
        // Arrange
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        // Seed a user
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

            db.Database.EnsureCreated();

            if (!db.Users.Any(u => u.Email == "test@example.com"))
            {
                var user = new ApplicationUser { UserName = "test@example.com", Email = "test@example.com", FirstName = "Test", LastName = "User" };
                await userManager.CreateAsync(user, "Password123!");
            }
        }

        // Get the login page to retrieve the anti-forgery token if necessary
        // (For simplicity in this basic example, we might skip it or use a library, 
        // but many default Identity pages require it. Let's try a direct POST first.)
        
        var initialResponse = await client.GetAsync("/Identity/Account/Login");
        initialResponse.EnsureSuccessStatusCode();
        var content = await initialResponse.Content.ReadAsStringAsync();
        
        // Extract anti-forgery token
        var tokenMatch = System.Text.RegularExpressions.Regex.Match(content, @"<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" />");
        var token = tokenMatch.Success ? tokenMatch.Groups[1].Value : "";
        
        var formModel = new Dictionary<string, string>
        {
            { "Input.Email", "test@example.com" },
            { "Input.Password", "Password123!" },
            { "Input.RememberMe", "false" },
            { "__RequestVerificationToken", token }
        };

        // Act
        var response = await client.PostAsync("/Identity/Account/Login", new FormUrlEncodedContent(formModel));

        // Assert
        // Redirect on success (usually to Home or ReturnUrl)
        Assert.Equal(System.Net.HttpStatusCode.Redirect, response.StatusCode);
    }
}
