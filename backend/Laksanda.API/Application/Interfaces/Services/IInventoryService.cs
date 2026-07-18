using Laksanda.API.Application.DTOs.Inventory;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<InventoryItemDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<InventoryItemDto?> GetByRawMaterialIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default);
}
