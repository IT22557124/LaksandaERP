using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class GoodsReceivedNoteRepository : IGoodsReceivedNoteRepository
{
    private readonly ApplicationDbContext _dbContext;

    public GoodsReceivedNoteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GoodsReceivedNote>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .Include(x => x.PurchaseOrder)
                .ThenInclude(x => x!.Supplier)
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .OrderByDescending(x => x.ReceivedDate)
            .ToListAsync(cancellationToken);
    }

    public Task<GoodsReceivedNote?> GetByIdAsync(Guid goodsReceivedNoteId, CancellationToken cancellationToken = default)
    {
        return _dbContext.GoodsReceivedNotes
            .Include(x => x.PurchaseOrder)
                .ThenInclude(x => x!.Supplier)
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .FirstOrDefaultAsync(x => x.GoodsReceivedNoteId == goodsReceivedNoteId, cancellationToken);
    }

    public Task<GoodsReceivedNote?> GetByGRNNumberAsync(string grnNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.GoodsReceivedNotes
            .FirstOrDefaultAsync(x => x.GRNNumber == grnNumber, cancellationToken);
    }

    public async Task AddAsync(GoodsReceivedNote goodsReceivedNote, CancellationToken cancellationToken = default)
    {
        await _dbContext.GoodsReceivedNotes.AddAsync(goodsReceivedNote, cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
