 using System.ComponentModel.DataAnnotations;


namespace ECommerceApp.Domain.Entities;

public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public required string Name { get; set; }
        
        [Required]
        public required string Description { get; set; }
        
        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, 99999)]
        public int Stock { get; set; }
        
        public required string ImageUrl { get; set; }
        
        public required string SellerId { get; set; }
        public virtual ApplicationUser? Seller { get; set; }
        
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
