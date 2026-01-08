// Data/ApplicationDbContext.cs
using ECommerceApp.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Lookup> Lookups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure relationships
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);

            // Seed categories
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and accessories" },
                new Category { Id = 2, Name = "Clothing", Description = "Men and women clothing" },
                new Category { Id = 3, Name = "Books", Description = "Books and magazines" },
                new Category { Id = 4, Name = "Home & Garden", Description = "Home and garden products" },
                new Category { Id = 5, Name = "Sports", Description = "Sports equipment and accessories" }
            );

            // Configure decimal precision
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            // Configure Lookup entity
            builder.Entity<Lookup>()
                .HasIndex(l => l.Key)
                .IsUnique();

            // Seed lookup data for application configuration
            builder.Entity<Lookup>().HasData(
                new Lookup { Id = 1, Key = "SiteName", Value = "ECommerce Store", Description = "Application name", Category = "General", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 2, Key = "AdminEmail", Value = "admin@ecommerce.com", Description = "Administrator email", Category = "General", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 3, Key = "Currency", Value = "USD", Description = "Default currency", Category = "Payment", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 4, Key = "TaxRate", Value = "0.10", Description = "Tax rate (10%)", Category = "Payment", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 5, Key = "ShippingFee", Value = "5.00", Description = "Standard shipping fee", Category = "Shipping", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 6, Key = "FreeShippingThreshold", Value = "50.00", Description = "Free shipping minimum order", Category = "Shipping", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 7, Key = "MaxCartItems", Value = "50", Description = "Maximum items in cart", Category = "Cart", IsActive = true, CreatedAt = DateTime.Now },
                new Lookup { Id = 8, Key = "ProductsPerPage", Value = "12", Description = "Products displayed per page", Category = "Display", IsActive = true, CreatedAt = DateTime.Now }
            );
        }
    }
}