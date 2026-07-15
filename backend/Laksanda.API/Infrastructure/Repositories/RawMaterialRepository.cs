using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class RawMaterialRepository : IRawMaterialRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RawMaterialRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<RawMaterial>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.RawMaterials
            .AsNoTracking()
            .OrderBy(x => x.MaterialCode)
            .ToListAsync(cancellationToken);
    }

    public Task<RawMaterial?> GetByIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default)
    {
        return _dbContext.RawMaterials.FirstOrDefaultAsync(x => x.RawMaterialId == rawMaterialId, cancellationToken);
    }

    public Task<RawMaterial?> GetByCodeAsync(string materialCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.RawMaterials.FirstOrDefaultAsync(x => x.MaterialCode == materialCode, cancellationToken);
    }

    public async Task AddAsync(RawMaterial rawMaterial, CancellationToken cancellationToken = default)
    {
        await _dbContext.RawMaterials.AddAsync(rawMaterial, cancellationToken);
    }

    public void Update(RawMaterial rawMaterial)
    {
        _dbContext.RawMaterials.Update(rawMaterial);
    }

    public void Delete(RawMaterial rawMaterial)
    {
        _dbContext.RawMaterials.Remove(rawMaterial);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
