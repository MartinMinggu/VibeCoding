 using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ECommerceApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public required string FirstName { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string LastName { get; set; }
        
        public string? Address { get; set; }
        public bool IsSeller { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
