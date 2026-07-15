using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task<Customer?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken = default);

    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task AddAsync(Customer customer, CancellationToken cancellationToken = default);

    void Update(Customer customer);

    void Delete(Customer customer);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
