using LazyLoadingInAspNetCoreWebAPI.Data;
using LazyLoadingInAspNetCoreWebAPI.DTOs;
using LazyLoadingInAspNetCoreWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LazyLoadingInAspNetCoreWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = _context.Products.ToList();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category?.Name // Triggers lazy loading
            }).ToList();

            return Ok(result);
        }

        // GET: api/products/1
        [HttpGet("{id}")]
        public ActionResult<ProductDto> GetProductById(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            // Accessing the Category navigation property triggers lazy loading
            var result = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                CategoryName = product.Category?.Name
            };

            return Ok(result);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            // Validate Category exists
            var category = await _context.Categories.FindAsync(product.CategoryId);
            if (category == null)
                return BadRequest("Invalid category ID.");

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // DELETE: api/products/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
