using Laksanda.API.Application.DTOs.Suppliers;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SupplierDto>>> GetAll([FromQuery] SupplierStatus? status, CancellationToken cancellationToken)
    {
        var suppliers = await _supplierService.GetAllAsync(status, cancellationToken);
        return Ok(suppliers);
    }

    [HttpGet("{supplierId:guid}")]
    public async Task<ActionResult<SupplierDto>> GetById(Guid supplierId, CancellationToken cancellationToken)
    {
        var supplier = await _supplierService.GetByIdAsync(supplierId, cancellationToken);
        if (supplier is null)
        {
            return NotFound();
        }

        return Ok(supplier);
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> Create([FromBody] CreateSupplierRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _supplierService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { supplierId = created.SupplierId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{supplierId:guid}")]
    public async Task<ActionResult<SupplierDto>> Update(Guid supplierId, [FromBody] UpdateSupplierRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _supplierService.UpdateAsync(supplierId, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{supplierId:guid}")]
    public async Task<IActionResult> Delete(Guid supplierId, CancellationToken cancellationToken)
    {
        var deleted = await _supplierService.DeleteAsync(supplierId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
