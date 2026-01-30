using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopSphere.API.DTOs;
using ShopSphere.Core.Entities;
using ShopSphere.Infrastructure.Data;

namespace ShopSphere.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/products?page=1&pageSize=10&search=laptop&categoryId=1
    [HttpGet]
    public async Task<ActionResult<ProductListResponse>> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? search = null,
        [FromQuery] int? categoryId = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive)
            .AsQueryable();

        // Filtres
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => 
                p.Name.ToLower().Contains(search.ToLower()) ||
                p.Description.ToLower().Contains(search.ToLower())
            );
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var products = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.ImageUrl,
                p.CategoryId,
                p.Category.Name,
                p.IsActive
            ))
            .ToListAsync();

        return Ok(new ProductListResponse(
            products,
            totalCount,
            page,
            pageSize,
            totalPages
        ));
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound(new { message = "Produit non trouvé" });
        }

        return Ok(new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.ImageUrl,
            product.CategoryId,
            product.Category.Name,
            product.IsActive
        ));
    }

    // POST: api/products
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        // Vérifier que la catégorie existe
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest(new { message = "Catégorie invalide" });
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            ImageUrl = dto.ImageUrl,
            CategoryId = dto.CategoryId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Recharger avec la catégorie
        await _context.Entry(product).Reference(p => p.Category).LoadAsync();

        var productDto = new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.ImageUrl,
            product.CategoryId,
            product.Category.Name,
            product.IsActive
        );

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { message = "Produit non trouvé" });
        }

        // Vérifier que la catégorie existe
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest(new { message = "Catégorie invalide" });
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.ImageUrl = dto.ImageUrl;
        product.CategoryId = dto.CategoryId;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound(new { message = "Produit non trouvé" });
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET: api/products/category/1
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<List<ProductDto>>> GetProductsByCategory(int categoryId)
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.Price,
                p.Stock,
                p.ImageUrl,
                p.CategoryId,
                p.Category.Name,
                p.IsActive
            ))
            .ToListAsync();

        return Ok(products);
    }
}
