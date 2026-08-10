namespace WarehouseApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Contracts;
using WarehouseApi.Models;
using WarehouseApi.Services;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger, IWebHostEnvironment env)
    {
        _productService = productService;
        _logger = logger;
        _env = env;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll([FromQuery] bool? onlyAvailable)
    {
        return Ok(_productService.GetAll(onlyAvailable));
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Product> GetById([FromRoute] Guid id)
    {
        var product = _productService.GetById(id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Product>> Search([FromQuery] string? name, [FromQuery] string? supplier)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(supplier))
        {
            return BadRequest("At least one search parameter ('name' or 'supplier') must be provided.");
        }

        return Ok(_productService.Search(name, supplier));
    }

    [HttpPost]
    public ActionResult<Product> Create([FromBody] CreateProductRequest request)
    {
        if (_productService.SkuExists(request.SKU))
        {
            return Conflict($"Product with SKU '{request.SKU}' already exists.");
        }

        var created = _productService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:guid}/quantity")]
    public IActionResult UpdateQuantity([FromRoute] Guid id, [FromBody] UpdateProductQuantityRequest request)
    {
        if (request.QuantityInStock < 0) return BadRequest("Quantity cannot be negative.");

        var success = _productService.UpdateQuantity(id, request.QuantityInStock);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpPost("{id:guid}/price")]
    public IActionResult UpdatePrice([FromRoute] Guid id, [FromBody] UpdateProductPriceRequest request)
    {
        if (request.Price <= 0) return BadRequest("Price must be greater than 0.");

        var product = _productService.GetById(id);
        if (product == null) return NotFound();

        decimal oldPrice = product.Price;
        _productService.UpdatePrice(id, request.Price);
        
        _logger.LogInformation("Product {Id} price updated from {OldPrice} to {NewPrice}", id, oldPrice, request.Price);

        return NoContent();
    }

    [HttpPost("{id:guid}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile file)
    {
        var product = _productService.GetById(id);
        if (product == null) return NotFound();

        if (file == null || file.Length == 0) return BadRequest("No file uploaded.");

        // Validate extension
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
        {
            return BadRequest("Only .jpg and .png images are allowed.");
        }

        // Validate size (max 2 MB)
        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest("File size exceeds 2 MB limit.");
        }

        var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var imageRecord = new ProductImage
        {
            ProductId = id,
            FileName = file.FileName,
            FilePath = $"/uploads/{uniqueFileName}"
        };

        _productService.AssignImage(id, imageRecord);

        return Ok(imageRecord);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var success = _productService.SoftDelete(id);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpGet("server-time")]
    public IActionResult GetServerTime([FromHeader(Name = "Accept-Language")] string? acceptLanguage)
    {
        var now = DateTime.Now;
        string formattedTime;

        try
        {
            var culture = new System.Globalization.CultureInfo(acceptLanguage ?? "en-US");
            formattedTime = now.ToString("f", culture);
        }
        catch
        {
            formattedTime = now.ToString("f", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
        }

        return Ok(new { Language = acceptLanguage ?? "en-US", ServerTime = formattedTime });
    }

    [HttpPost("{id:guid}/assign-supplier/{supplierId:guid}")]
    public IActionResult AssignSupplier([FromRoute] Guid id, [FromRoute] Guid supplierId)
    {
        var success = _productService.AssignSupplier(id, supplierId);
        if (!success) return BadRequest("Assignment failed. Check if product/supplier exist, product is not archived, or supplier is active.");

        return NoContent();
    }
}