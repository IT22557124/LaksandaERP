using Laksanda.API.Application.DTOs.Suppliers;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.Interfaces.Services;

public interface ISupplierService
{
    Task<IReadOnlyList<SupplierDto>> GetAllAsync(SupplierStatus? status, CancellationToken cancellationToken = default);

    Task<SupplierDto?> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default);

    Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default);

    Task<SupplierDto?> UpdateAsync(Guid supplierId, UpdateSupplierRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid supplierId, CancellationToken cancellationToken = default);
}
