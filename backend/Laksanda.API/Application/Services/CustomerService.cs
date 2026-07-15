using Laksanda.API.Application.DTOs.Customers;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);
        return customers.Select(MapToDto).ToArray();
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        return customer is null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var errors = CustomerValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedCode = request.CustomerCode.Trim();
        var existingByCode = await _customerRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null)
        {
            throw new ArgumentException("CustomerCode already exists.");
        }

        var normalizedEmail = request.Email?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedEmail))
        {
            var existingByEmail = await _customerRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (existingByEmail is not null)
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        var customer = new Customer
        {
            CustomerCode = normalizedCode,
            CustomerName = request.CustomerName.Trim(),
            Phone = request.Phone?.Trim(),
            Address = request.Address?.Trim(),
            Email = normalizedEmail,
            CreditLimit = request.CreditLimit
        };

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(customer);
    }

    public async Task<CustomerDto?> UpdateAsync(Guid customerId, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        var errors = CustomerValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var normalizedCode = request.CustomerCode.Trim();
        var existingByCode = await _customerRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null && existingByCode.CustomerId != customerId)
        {
            throw new ArgumentException("CustomerCode already exists.");
        }

        var normalizedEmail = request.Email?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedEmail))
        {
            var existingByEmail = await _customerRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (existingByEmail is not null && existingByEmail.CustomerId != customerId)
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        customer.CustomerCode = normalizedCode;
        customer.CustomerName = request.CustomerName.Trim();
        customer.Phone = request.Phone?.Trim();
        customer.Address = request.Address?.Trim();
        customer.Email = normalizedEmail;
        customer.CreditLimit = request.CreditLimit;

        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(customer);
    }

    public async Task<bool> DeleteAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            return false;
        }

        _customerRepository.Delete(customer);
        await _customerRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static CustomerDto MapToDto(Customer customer)
    {
        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            CustomerCode = customer.CustomerCode,
            CustomerName = customer.CustomerName,
            Phone = customer.Phone,
            Address = customer.Address,
            Email = customer.Email,
            CreditLimit = customer.CreditLimit
        };
    }
}
