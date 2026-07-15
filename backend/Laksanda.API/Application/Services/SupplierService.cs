using Laksanda.API.Application.DTOs.Suppliers;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierService(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<IReadOnlyList<SupplierDto>> GetAllAsync(SupplierStatus? status, CancellationToken cancellationToken = default)
    {
        var suppliers = await _supplierRepository.GetAllAsync(status, cancellationToken);
        return suppliers.Select(MapToDto).ToArray();
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        return supplier is null ? null : MapToDto(supplier);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        var errors = SupplierValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var existingByCode = await _supplierRepository.GetByCodeAsync(request.SupplierCode.Trim(), cancellationToken);
        if (existingByCode is not null)
        {
            throw new ArgumentException("SupplierCode already exists.");
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingByEmail = await _supplierRepository.GetByEmailAsync(request.Email.Trim(), cancellationToken);
            if (existingByEmail is not null)
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        var supplier = new Supplier
        {
            SupplierCode = request.SupplierCode.Trim(),
            SupplierName = request.SupplierName.Trim(),
            Phone = request.Phone?.Trim(),
            Email = request.Email?.Trim(),
            Address = request.Address?.Trim(),
            Status = request.Status
        };

        await _supplierRepository.AddAsync(supplier, cancellationToken);
        await _supplierRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(supplier);
    }

    public async Task<SupplierDto?> UpdateAsync(Guid supplierId, UpdateSupplierRequest request, CancellationToken cancellationToken = default)
    {
        var errors = SupplierValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var supplier = await _supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        if (supplier is null)
        {
            return null;
        }

        var normalizedCode = request.SupplierCode.Trim();
        var existingByCode = await _supplierRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null && existingByCode.SupplierId != supplierId)
        {
            throw new ArgumentException("SupplierCode already exists.");
        }

        var normalizedEmail = request.Email?.Trim();
        if (!string.IsNullOrWhiteSpace(normalizedEmail))
        {
            var existingByEmail = await _supplierRepository.GetByEmailAsync(normalizedEmail, cancellationToken);
            if (existingByEmail is not null && existingByEmail.SupplierId != supplierId)
            {
                throw new ArgumentException("Email already exists.");
            }
        }

        supplier.SupplierCode = normalizedCode;
        supplier.SupplierName = request.SupplierName.Trim();
        supplier.Phone = request.Phone?.Trim();
        supplier.Email = normalizedEmail;
        supplier.Address = request.Address?.Trim();
        supplier.Status = request.Status;

        _supplierRepository.Update(supplier);
        await _supplierRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(supplier);
    }

    public async Task<bool> DeleteAsync(Guid supplierId, CancellationToken cancellationToken = default)
    {
        var supplier = await _supplierRepository.GetByIdAsync(supplierId, cancellationToken);
        if (supplier is null)
        {
            return false;
        }

        _supplierRepository.Delete(supplier);
        await _supplierRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            SupplierId = supplier.SupplierId,
            SupplierCode = supplier.SupplierCode,
            SupplierName = supplier.SupplierName,
            Phone = supplier.Phone,
            Email = supplier.Email,
            Address = supplier.Address,
            Status = supplier.Status
        };
    }
}
