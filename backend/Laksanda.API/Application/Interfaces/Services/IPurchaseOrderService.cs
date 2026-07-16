using Laksanda.API.Application.DTOs.PurchaseOrders;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IPurchaseOrderService
{
    Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto?> GetByIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default);

    Task<PurchaseOrderDto?> UpdateAsync(Guid purchaseOrderId, UpdatePurchaseOrderRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default);
}
