using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property: one User has many Categories
        public List<Category> Categories { get; set; } = new();
    }
}