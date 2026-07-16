using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface IPurchaseOrderRepository
{
    Task<IReadOnlyList<PurchaseOrder>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PurchaseOrder?> GetByIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);

    Task<PurchaseOrder?> GetByIdForUpdateAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);

    Task<PurchaseOrder?> GetByPONumberAsync(string poNumber, CancellationToken cancellationToken = default);

    Task AddAsync(PurchaseOrder purchaseOrder, CancellationToken cancellationToken = default);

    Task DeleteItemsByPurchaseOrderIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);

    Task AddItemsAsync(IEnumerable<PurchaseOrderItem> items, CancellationToken cancellationToken = default);

    void Update(PurchaseOrder purchaseOrder);

    void Delete(PurchaseOrder purchaseOrder);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
