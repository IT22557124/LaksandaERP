using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface IRawMaterialRepository
{
    Task<IReadOnlyList<RawMaterial>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<RawMaterial?> GetByIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default);

    Task<RawMaterial?> GetByCodeAsync(string materialCode, CancellationToken cancellationToken = default);

    Task AddAsync(RawMaterial rawMaterial, CancellationToken cancellationToken = default);

    void Update(RawMaterial rawMaterial);

    void Delete(RawMaterial rawMaterial);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
