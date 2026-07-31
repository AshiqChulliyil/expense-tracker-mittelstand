using System.Runtime.CompilerServices;
using ExpenseTracker.Api.Models;

namespace ExpenseTracker.Api.DTOs
{
    public class  InvoiceDto
    {
        public int Id { get; set; }
        public string VendorName { get; set; } = String.Empty;
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Description { get; set; }
        public string? ReceiptFilePath { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}