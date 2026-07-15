using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Laksanda.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SupplierRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Supplier>> GetAllAsync(SupplierStatus? status, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Suppliers.AsNoTracking().AsQueryable();

        if (status.HasValue)
        {
            query = query.Where(x => x.Status == status.Value);
        }

        return await query
            .OrderBy(x => x.SupplierCode)
            .ToListAsync(cancellationToken);
    }

    public Task<Supplier?> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == supplierId, cancellationToken);
    }

    public Task<Supplier?> GetByCodeAsync(string supplierCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Suppliers.FirstOrDefaultAsync(x => x.SupplierCode == supplierCode, cancellationToken);
    }

    public Task<Supplier?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Suppliers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        await _dbContext.Suppliers.AddAsync(supplier, cancellationToken);
    }

    public void Update(Supplier supplier)
    {
        _dbContext.Suppliers.Update(supplier);
    }

    public void Delete(Supplier supplier)
    {
        _dbContext.Suppliers.Remove(supplier);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
