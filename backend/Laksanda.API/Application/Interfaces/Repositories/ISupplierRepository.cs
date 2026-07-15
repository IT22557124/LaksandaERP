using Laksanda.API.Domain.Entities;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface ISupplierRepository
{
    Task<IReadOnlyList<Supplier>> GetAllAsync(SupplierStatus? status, CancellationToken cancellationToken = default);

    Task<Supplier?> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default);

    Task<Supplier?> GetByCodeAsync(string supplierCode, CancellationToken cancellationToken = default);

    Task<Supplier?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);

    void Update(Supplier supplier);

    void Delete(Supplier supplier);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
