using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PurchaseOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(x => x.Supplier)
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .OrderByDescending(x => x.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public Task<PurchaseOrder?> GetByIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
    {
        return _dbContext.PurchaseOrders
            .Include(x => x.Supplier)
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .FirstOrDefaultAsync(x => x.PurchaseOrderId == purchaseOrderId, cancellationToken);
    }

    public Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, CancellationToken cancellationToken = default)
    {
        return _dbContext.PurchaseOrders
            .FirstOrDefaultAsync(x => x.PONumber == poNumber, cancellationToken);
    }

    public async Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default)
    {
        await _dbContext.PurchaseOrders.AddAsync(purchaseOrder, cancellationToken);
    }

    public void Update(PurchaseOrder purchaseOrder)
    {
        _dbContext.PurchaseOrders.Update(purchaseOrder);
    }

    public void Delete(PurchaseOrder purchaseOrder)
    {
        _dbContext.PurchaseOrders.Remove(purchaseOrder);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
