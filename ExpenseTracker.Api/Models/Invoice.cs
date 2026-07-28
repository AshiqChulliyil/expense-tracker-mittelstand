using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Api.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        public string VendorName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime InvoiceDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string? Description { get; set; }

        public string? ReceiptFilePath { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public User? User { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}