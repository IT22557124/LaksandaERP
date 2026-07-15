using Laksanda.API.Application.DTOs.RawMaterials;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IRawMaterialService
{
    Task<IReadOnlyList<RawMaterialDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<RawMaterialDto?> GetByIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default);

    Task<RawMaterialDto> CreateAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default);

    Task<RawMaterialDto?> UpdateAsync(Guid rawMaterialId, UpdateRawMaterialRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid rawMaterialId, CancellationToken cancellationToken = default);
}
