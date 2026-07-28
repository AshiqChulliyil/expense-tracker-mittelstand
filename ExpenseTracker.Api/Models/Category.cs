using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Api.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyBudgetLimit { get; set; }

        // Foreign key: which User owns this category
        public int UserId { get; set; }
        public User? User { get; set; }

        // Navigation property: one Category has many Invoices
        public List<Invoice> Invoices { get; set; } = new();
    }
}