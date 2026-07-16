using Laksanda.API.Application.DTOs.GoodsReceivedNotes;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IGoodsReceivedNoteService
{
    Task<IReadOnlyList<GoodsReceivedNoteDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GoodsReceivedNoteDto?> GetByIdAsync(Guid goodsReceivedNoteId, CancellationToken cancellationToken = default);

    Task<GoodsReceivedNoteDto> CreateAsync(CreateGoodsReceivedNoteRequest request, CancellationToken cancellationToken = default);
}
