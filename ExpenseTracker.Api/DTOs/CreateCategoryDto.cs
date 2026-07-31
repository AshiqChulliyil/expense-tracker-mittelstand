using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Api.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal MonthlyBudgetLimit { get; set; }
    }
}