using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .OrderBy(x => x.CustomerCode)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerId == customerId, cancellationToken);
    }

    public Task<Customer?> GetByCodeAsync(string customerCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(x => x.CustomerCode == customerCode, cancellationToken);
    }

    public Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        await _dbContext.Customers.AddAsync(customer, cancellationToken);
    }

    public void Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
    }

    public void Delete(Customer customer)
    {
        _dbContext.Customers.Remove(customer);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
