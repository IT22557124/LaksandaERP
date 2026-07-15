using Laksanda.API.Application.DTOs.RawMaterials;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class RawMaterialService : IRawMaterialService
{
    private readonly IRawMaterialRepository _rawMaterialRepository;

    public RawMaterialService(IRawMaterialRepository rawMaterialRepository)
    {
        _rawMaterialRepository = rawMaterialRepository;
    }

    public async Task<IReadOnlyList<RawMaterialDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rawMaterials = await _rawMaterialRepository.GetAllAsync(cancellationToken);
        return rawMaterials.Select(MapToDto).ToArray();
    }

    public async Task<RawMaterialDto?> GetByIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default)
    {
        var rawMaterial = await _rawMaterialRepository.GetByIdAsync(rawMaterialId, cancellationToken);
        return rawMaterial is null ? null : MapToDto(rawMaterial);
    }

    public async Task<RawMaterialDto> CreateAsync(CreateRawMaterialRequest request, CancellationToken cancellationToken = default)
    {
        var errors = RawMaterialValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedCode = request.MaterialCode.Trim();
        var existingByCode = await _rawMaterialRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null)
        {
            throw new ArgumentException("MaterialCode already exists.");
        }

        var rawMaterial = new RawMaterial
        {
            MaterialCode = normalizedCode,
            MaterialName = request.MaterialName.Trim(),
            Unit = request.Unit?.Trim(),
            CurrentStock = request.CurrentStock,
            ReorderLevel = request.ReorderLevel,
            Cost = request.Cost,
            Status = request.Status
        };

        await _rawMaterialRepository.AddAsync(rawMaterial, cancellationToken);
        await _rawMaterialRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(rawMaterial);
    }

    public async Task<RawMaterialDto?> UpdateAsync(Guid rawMaterialId, UpdateRawMaterialRequest request, CancellationToken cancellationToken = default)
    {
        var errors = RawMaterialValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var rawMaterial = await _rawMaterialRepository.GetByIdAsync(rawMaterialId, cancellationToken);
        if (rawMaterial is null)
        {
            return null;
        }

        var normalizedCode = request.MaterialCode.Trim();
        var existingByCode = await _rawMaterialRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null && existingByCode.RawMaterialId != rawMaterialId)
        {
            throw new ArgumentException("MaterialCode already exists.");
        }

        rawMaterial.MaterialCode = normalizedCode;
        rawMaterial.MaterialName = request.MaterialName.Trim();
        rawMaterial.Unit = request.Unit?.Trim();
        rawMaterial.CurrentStock = request.CurrentStock;
        rawMaterial.ReorderLevel = request.ReorderLevel;
        rawMaterial.Cost = request.Cost;
        rawMaterial.Status = request.Status;

        _rawMaterialRepository.Update(rawMaterial);
        await _rawMaterialRepository.SaveChangesAsync(cancellationToken);

        return MapToDto(rawMaterial);
    }

    public async Task<bool> DeleteAsync(Guid rawMaterialId, CancellationToken cancellationToken = default)
    {
        var rawMaterial = await _rawMaterialRepository.GetByIdAsync(rawMaterialId, cancellationToken);
        if (rawMaterial is null)
        {
            return false;
        }

        _rawMaterialRepository.Delete(rawMaterial);
        await _rawMaterialRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static RawMaterialDto MapToDto(RawMaterial rawMaterial)
    {
        return new RawMaterialDto
        {
            RawMaterialId = rawMaterial.RawMaterialId,
            MaterialCode = rawMaterial.MaterialCode,
            MaterialName = rawMaterial.MaterialName,
            Unit = rawMaterial.Unit,
            CurrentStock = rawMaterial.CurrentStock,
            ReorderLevel = rawMaterial.ReorderLevel,
            Cost = rawMaterial.Cost,
            Status = rawMaterial.Status
        };
    }
}
