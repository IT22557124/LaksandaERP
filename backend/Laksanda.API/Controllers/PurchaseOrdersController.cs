using Laksanda.API.Application.DTOs.PurchaseOrders;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrdersController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PurchaseOrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var purchaseOrders = await _purchaseOrderService.GetAllAsync(cancellationToken);
        return Ok(purchaseOrders);
    }

    [HttpGet("{purchaseOrderId:guid}")]
    public async Task<ActionResult<PurchaseOrderDto>> GetById(Guid purchaseOrderId, CancellationToken cancellationToken)
    {
        var purchaseOrder = await _purchaseOrderService.GetByIdAsync(purchaseOrderId, cancellationToken);
        if (purchaseOrder is null)
        {
            return NotFound();
        }

        return Ok(purchaseOrder);
    }

    [HttpPost]
    public async Task<ActionResult<PurchaseOrderDto>> Create([FromBody] CreatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _purchaseOrderService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { purchaseOrderId = created.PurchaseOrderId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { message = "Purchase order was modified or removed by another operation. Refresh and try again." });
        }
    }

    [HttpPut("{purchaseOrderId:guid}")]
    public async Task<ActionResult<PurchaseOrderDto>> Update(Guid purchaseOrderId, [FromBody] UpdatePurchaseOrderRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _purchaseOrderService.UpdateAsync(purchaseOrderId, request, cancellationToken);
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

    [HttpDelete("{purchaseOrderId:guid}")]
    public async Task<IActionResult> Delete(Guid purchaseOrderId, CancellationToken cancellationToken)
    {
        var deleted = await _purchaseOrderService.DeleteAsync(purchaseOrderId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
