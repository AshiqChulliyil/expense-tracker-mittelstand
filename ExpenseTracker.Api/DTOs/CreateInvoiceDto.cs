using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Api.Models;

namespace ExpenseTracker.Api.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public string VendorName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string? Description { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

    }
}