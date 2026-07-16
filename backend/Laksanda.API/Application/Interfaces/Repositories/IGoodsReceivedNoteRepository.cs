using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface IGoodsReceivedNoteRepository
{
    Task<IReadOnlyList<GoodsReceivedNote>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<GoodsReceivedNote?> GetByIdAsync(Guid goodsReceivedNoteId, CancellationToken cancellationToken = default);

    Task<GoodsReceivedNote?> GetByGRNNumberAsync(string grnNumber, CancellationToken cancellationToken = default);

    Task AddAsync(GoodsReceivedNote goodsReceivedNote, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
