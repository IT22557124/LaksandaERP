using Laksanda.API.Application.DTOs.Customers;

namespace Laksanda.API.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CustomerDto?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<CustomerDto?> UpdateAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid customerId, CancellationToken cancellationToken = default);
}
