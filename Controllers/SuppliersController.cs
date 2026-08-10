namespace WarehouseApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WarehouseApi.Contracts;
using WarehouseApi.Models;
using WarehouseApi.Services;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Supplier>> GetAll() => Ok(_supplierService.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<Supplier> GetById([FromRoute] Guid id)
    {
        var supplier = _supplierService.GetById(id);
        if (supplier == null) return NotFound();
        return Ok(supplier);
    }

    [HttpPost]
    public ActionResult<Supplier> Create([FromBody] CreateSupplierRequest request)
    {
        var supplier = _supplierService.Create(request);
        return CreatedAtAction(nameof(GetById), new { id = supplier.Id }, supplier);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Deactivate([FromRoute] Guid id)
    {
        var success = _supplierService.Deactivate(id);
        if (!success) return NotFound();
        return NoContent();
    }
}