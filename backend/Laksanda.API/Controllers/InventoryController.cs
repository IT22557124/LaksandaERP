using Laksanda.API.Application.DTOs.Inventory;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<InventoryItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        var inventory = await _inventoryService.GetAllAsync(cancellationToken);
        return Ok(inventory);
    }

    [HttpGet("{rawMaterialId:guid}")]
    public async Task<ActionResult<InventoryItemDto>> GetByRawMaterialId(Guid rawMaterialId, CancellationToken cancellationToken)
    {
        var item = await _inventoryService.GetByRawMaterialIdAsync(rawMaterialId, cancellationToken);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }
}
