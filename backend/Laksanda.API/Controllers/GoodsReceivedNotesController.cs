using Laksanda.API.Application.DTOs.GoodsReceivedNotes;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoodsReceivedNotesController : ControllerBase
{
    private readonly IGoodsReceivedNoteService _goodsReceivedNoteService;

    public GoodsReceivedNotesController(IGoodsReceivedNoteService goodsReceivedNoteService)
    {
        _goodsReceivedNoteService = goodsReceivedNoteService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GoodsReceivedNoteDto>>> GetAll(CancellationToken cancellationToken)
    {
        var notes = await _goodsReceivedNoteService.GetAllAsync(cancellationToken);
        return Ok(notes);
    }

    [HttpGet("{goodsReceivedNoteId:guid}")]
    public async Task<ActionResult<GoodsReceivedNoteDto>> GetById(Guid goodsReceivedNoteId, CancellationToken cancellationToken)
    {
        var note = await _goodsReceivedNoteService.GetByIdAsync(goodsReceivedNoteId, cancellationToken);
        if (note is null)
        {
            return NotFound();
        }

        return Ok(note);
    }

    [HttpPost]
    public async Task<ActionResult<GoodsReceivedNoteDto>> Create([FromBody] CreateGoodsReceivedNoteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _goodsReceivedNoteService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { goodsReceivedNoteId = created.GoodsReceivedNoteId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
