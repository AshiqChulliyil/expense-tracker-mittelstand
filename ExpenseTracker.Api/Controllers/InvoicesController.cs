using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ExpenseTracker.Api.Data;
using ExpenseTracker.Api.Models;
using ExpenseTracker.Api.DTOs;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(userIdClaim);
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices()
        {
            var userId = GetCurrentUserId();

            var invoices = await _context.Invoices
                .Where(i => i.UserId == userId)
                .Include(i => i.Category)
                .Select(i => new InvoiceDto
                {
                    Id = i.Id,
                    VendorName = i.VendorName,
                    Amount = i.Amount,
                    InvoiceDate = i.InvoiceDate,
                    PaymentMethod = i.PaymentMethod,
                    Description = i.Description,
                    ReceiptFilePath = i.ReceiptFilePath,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category != null ? i.Category.Name : string.Empty
                })
                .ToListAsync();

            return Ok(invoices);
        }

        // GET: api/invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice(int id)
        {
            var userId = GetCurrentUserId();

            var invoice = await _context.Invoices
                .Include(i => i.Category)
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (invoice == null)
                return NotFound();

            var dto = new InvoiceDto
            {
                Id = invoice.Id,
                VendorName = invoice.VendorName,
                Amount = invoice.Amount,
                InvoiceDate = invoice.InvoiceDate,
                PaymentMethod = invoice.PaymentMethod,
                Description = invoice.Description,
                ReceiptFilePath = invoice.ReceiptFilePath,
                CategoryId = invoice.CategoryId,
                CategoryName = invoice.Category != null ? invoice.Category.Name : string.Empty
            };

            return Ok(dto);
        }

        // POST: api/invoices
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice(CreateInvoiceDto createDto)
        {
            var userId = GetCurrentUserId();

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == createDto.CategoryId && c.UserId == userId);
            if (!categoryExists)
                return BadRequest($"Category with id {createDto.CategoryId} does not exist.");

            var invoice = new Invoice
            {
                VendorName = createDto.VendorName,
                Amount = createDto.Amount,
                InvoiceDate = createDto.InvoiceDate,
                PaymentMethod = createDto.PaymentMethod,
                Description = createDto.Description,
                CategoryId = createDto.CategoryId,
                UserId = userId
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            var category = await _context.Categories.FindAsync(invoice.CategoryId);

            var dto = new InvoiceDto
            {
                Id = invoice.Id,
                VendorName = invoice.VendorName,
                Amount = invoice.Amount,
                InvoiceDate = invoice.InvoiceDate,
                PaymentMethod = invoice.PaymentMethod,
                Description = invoice.Description,
                ReceiptFilePath = invoice.ReceiptFilePath,
                CategoryId = invoice.CategoryId,
                CategoryName = category?.Name ?? string.Empty
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, dto);
        }

        // PUT: api/invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, CreateInvoiceDto updateDto)
        {
            var userId = GetCurrentUserId();

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (invoice == null)
                return NotFound();

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == updateDto.CategoryId && c.UserId == userId);
            if (!categoryExists)
                return BadRequest($"Category with id {updateDto.CategoryId} does not exist.");

            invoice.VendorName = updateDto.VendorName;
            invoice.Amount = updateDto.Amount;
            invoice.InvoiceDate = updateDto.InvoiceDate;
            invoice.PaymentMethod = updateDto.PaymentMethod;
            invoice.Description = updateDto.Description;
            invoice.CategoryId = updateDto.CategoryId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var userId = GetCurrentUserId();

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}