using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_project.Models;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    private readonly PostgresdbContext _context;

    public WarehouseController(PostgresdbContext context)
    {
        _context = context;
    }

    // 1. Get products by supplier name (ordered by CreatedAt or Name)
    [HttpGet("supplier/{supplierName}")]
    public async Task<IActionResult> GetProductsBySupplier(string supplierName)
    {
        var products = await _context.Products
            .Include(p => p.Supplier)
            .Where(p => p.Supplier != null && p.Supplier.Name.ToLower() == supplierName.ToLower())
            .ToListAsync();

        return Ok(products);
    }

    // 2. Group products by expiry year
    [HttpGet("group/expiry-year")]
    public async Task<IActionResult> GroupProductsByExpiryYear()
    {
        var grouped = await _context.Products
            .GroupBy(p => p.ExpiryYear)
            .Select(g => new {
                ExpiryYear = g.Key,
                Count = g.Count(),
                Products = g.ToList()
            })
            .ToListAsync();

        return Ok(grouped);
    }

    // 3. Group products by expiry year and supplier country
    [HttpGet("group/expiry-and-country")]
    public async Task<IActionResult> GroupProductsByExpiryAndCountry()
    {
        var grouped = await _context.Products
            .Include(p => p.Supplier)
            .GroupBy(p => new { 
                p.ExpiryYear, 
                Country = p.Supplier != null ? p.Supplier.Country : "Unknown" 
            })
            .Select(g => new {
                g.Key.ExpiryYear,
                g.Key.Country,
                Count = g.Count(),
                Products = g.ToList()
            })
            .ToListAsync();

        return Ok(grouped);
    }

    // 4. Get total product count
    [HttpGet("products/count")]
    public async Task<IActionResult> GetTotalProductCount()
    {
        int count = await _context.Products.CountAsync();
        return Ok(new { TotalProducts = count });
    }

    // 5. Server-side pagination
    [HttpGet("products/paginated")]
    public async Task<IActionResult> GetPaginatedProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 5;

        var totalRecords = await _context.Products.CountAsync();
        
        var products = await _context.Products
            .Include(p => p.Supplier)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
            Data = products
        });
    }
}