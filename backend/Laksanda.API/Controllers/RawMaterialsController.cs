using Laksanda.API.Application.DTOs.RawMaterials;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RawMaterialsController : ControllerBase
{
    private readonly IRawMaterialService _rawMaterialService;

    public RawMaterialsController(IRawMaterialService rawMaterialService)
    {
        _rawMaterialService = rawMaterialService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RawMaterialDto>>> GetAll(CancellationToken cancellationToken)
    {
        var rawMaterials = await _rawMaterialService.GetAllAsync(cancellationToken);
        return Ok(rawMaterials);
    }

    [HttpGet("{rawMaterialId:guid}")]
    public async Task<ActionResult<RawMaterialDto>> GetById(Guid rawMaterialId, CancellationToken cancellationToken)
    {
        var rawMaterial = await _rawMaterialService.GetByIdAsync(rawMaterialId, cancellationToken);
        if (rawMaterial is null)
        {
            return NotFound();
        }

        return Ok(rawMaterial);
    }

    [HttpPost]
    public async Task<ActionResult<RawMaterialDto>> Create([FromBody] CreateRawMaterialRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _rawMaterialService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { rawMaterialId = created.RawMaterialId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{rawMaterialId:guid}")]
    public async Task<ActionResult<RawMaterialDto>> Update(Guid rawMaterialId, [FromBody] UpdateRawMaterialRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _rawMaterialService.UpdateAsync(rawMaterialId, request, cancellationToken);
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

    [HttpDelete("{rawMaterialId:guid}")]
    public async Task<IActionResult> Delete(Guid rawMaterialId, CancellationToken cancellationToken)
    {
        var deleted = await _rawMaterialService.DeleteAsync(rawMaterialId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
