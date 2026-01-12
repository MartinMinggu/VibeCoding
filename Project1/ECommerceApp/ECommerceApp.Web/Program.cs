using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.Infrastructure.Data;
using ECommerceApp.Domain.Entities;
using ECommerceApp.Domain.Interfaces;
using ECommerceApp.Application.Services;
using ECommerceApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Register Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddControllersWithViews()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddRazorPages();
builder.Services.AddSignalR();



builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "id-ID", "en-US" };
    
    var idCulture = new System.Globalization.CultureInfo("id-ID");
    idCulture.NumberFormat.NumberDecimalSeparator = ".";
    idCulture.NumberFormat.CurrencyDecimalSeparator = ".";
    idCulture.NumberFormat.NumberGroupSeparator = ",";
    idCulture.NumberFormat.CurrencyGroupSeparator = ",";

    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(idCulture);
    options.SupportedCultures = new List<System.Globalization.CultureInfo> { idCulture, new System.Globalization.CultureInfo("en-US") };
    options.SupportedUICultures = new List<System.Globalization.CultureInfo> { idCulture, new System.Globalization.CultureInfo("en-US") };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Handle status codes (404, 403, etc.) with custom pages
app.UseStatusCodePagesWithReExecute("/Error/{0}");



app.UseAuthorization();

// Seed roles and ContentManager user
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    await dbContext.Database.MigrateAsync();
    
    string[] roles = { "Admin", "ContentManager", "Seller", "Customer" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
    
    // Create ContentManager user
    var contentManagerEmail = "contentmanager@ecommerce.com";
    var contentManagerUser = await userManager.FindByEmailAsync(contentManagerEmail);
    if (contentManagerUser == null)
    {
        contentManagerUser = new ApplicationUser
        {
            UserName = contentManagerEmail,
            Email = contentManagerEmail,
            FirstName = "Content",
            LastName = "Manager",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(contentManagerUser, "Content123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(contentManagerUser, "ContentManager");
        }
    }
    
    // Seed sample announcements
    if (!dbContext.Announcements.Any())
    {
        var announcements = new[]
        {
            new Announcement
            {
                Title = "🎉 Grand Opening Sale!",
                Content = "Welcome to ECommerceApp! Enjoy 20% off on all products this week. Use code WELCOME20 at checkout.",
                Type = AnnouncementType.Promo,
                IsActive = true,
                CreatedById = contentManagerUser.Id,
                CreatedAt = DateTime.Now
            },
            new Announcement
            {
                Title = "New Products Arrived",
                Content = "Check out our latest collection of electronics and fashion items. Fresh arrivals every week!",
                Type = AnnouncementType.News,
                IsActive = true,
                CreatedById = contentManagerUser.Id,
                CreatedAt = DateTime.Now
            },
            new Announcement
            {
                Title = "Flash Sale Event - January 15th",
                Content = "Mark your calendars! Massive discounts up to 50% on selected items. Limited time only.",
                Type = AnnouncementType.Event,
                StartDate = new DateTime(2026, 1, 15),
                EndDate = new DateTime(2026, 1, 20),
                IsActive = true,
                CreatedById = contentManagerUser.Id,
                CreatedAt = DateTime.Now
            }
        };
        dbContext.Announcements.AddRange(announcements);
        await dbContext.SaveChangesAsync();
    }
    
    // Create Seller user and sample products
    var sellerEmail = "seller@ecommerce.com";
    var sellerUser = await userManager.FindByEmailAsync(sellerEmail);
    if (sellerUser == null)
    {
        sellerUser = new ApplicationUser
        {
            UserName = sellerEmail,
            Email = sellerEmail,
            FirstName = "Demo",
            LastName = "Seller",
            IsSeller = true,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(sellerUser, "Seller123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(sellerUser, "Seller");
        }
    }
    
    // Seed sample products (100 products)
    await ECommerceApp.Web.Data.ProductSeeder.SeedProductsAsync(dbContext, sellerUser.Id);
    
    // Seed 5 new sellers and 100 new products
    var newSellerIds = await ECommerceApp.Web.Data.SellerSeeder.SeedSellersAsync(userManager);
    if (newSellerIds.Count > 0)
    {
        await ECommerceApp.Web.Data.NewProductSeeder.SeedNewProductsAsync(dbContext, newSellerIds);
    }
    
    // Seed buyer accounts
    // TODO: Fix BuyerSeeder error before uncommenting
    // await ECommerceApp.Web.Data.BuyerSeeder.SeedBuyersAsync(userManager);
}

app.MapRazorPages();
app.MapHub<ECommerceApp.Web.Hubs.ChatHub>("/chathub");

// Area routing
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }
