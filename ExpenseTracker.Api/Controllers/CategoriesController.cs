using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.DTOs;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                Id = c.Id,
                Name = c.Name,
                MonthlyBudgetLimit = c.MonthlyBudgetLimit
                })
                .ToListAsync();
            return Ok(categories);

        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                MonthlyBudgetLimit = category.MonthlyBudgetLimit
            };

            return Ok(dto);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryDto createDto)
        {
            var category = new Category
            {
                Name = createDto.Name,
                MonthlyBudgetLimit = createDto.MonthlyBudgetLimit,
                UserId = 1 // temporary hardcoded value until Day 3 auth is added
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                MonthlyBudgetLimit = category.MonthlyBudgetLimit
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, dto);

        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CreateCategoryDto updateDto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            category.Name = updateDto.Name;
            category.MonthlyBudgetLimit = updateDto.MonthlyBudgetLimit;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}