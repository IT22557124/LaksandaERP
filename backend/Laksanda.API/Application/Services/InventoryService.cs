using Laksanda.API.Application.DTOs.Inventory;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _dbContext;

    public InventoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<InventoryItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var rawMaterials = await _dbContext.RawMaterials
            .AsNoTracking()
            .OrderBy(x => x.MaterialCode)
            .ToListAsync(cancellationToken);

        var stockInByMaterial = await _dbContext.GoodsReceivedNoteItems
            .AsNoTracking()
            .GroupBy(x => x.RawMaterialId)
            .Select(group => new { RawMaterialId = group.Key, StockIn = group.Sum(x => x.ReceivedQuantity) })
            .ToDictionaryAsync(x => x.RawMaterialId, x => x.StockIn, cancellationToken);

        return rawMaterials
            .Select(rawMaterial => MapToDto(rawMaterial, stockInByMaterial))
            .ToArray();
    }

    public async Task<InventoryItemDto?> GetByRawMaterialIdAsync(Guid rawMaterialId, CancellationToken cancellationToken = default)
    {
        var rawMaterial = await _dbContext.RawMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RawMaterialId == rawMaterialId, cancellationToken);

        if (rawMaterial is null)
        {
            return null;
        }

        var stockIn = await _dbContext.GoodsReceivedNoteItems
            .AsNoTracking()
            .Where(x => x.RawMaterialId == rawMaterialId)
            .SumAsync(x => x.ReceivedQuantity, cancellationToken);

        return MapToDto(rawMaterial, stockIn);
    }

    private static InventoryItemDto MapToDto(RawMaterial rawMaterial, IReadOnlyDictionary<Guid, decimal> stockInByMaterial)
    {
        var stockIn = stockInByMaterial.GetValueOrDefault(rawMaterial.RawMaterialId);
        return MapToDto(rawMaterial, stockIn);
    }

    private static InventoryItemDto MapToDto(RawMaterial rawMaterial, decimal stockIn)
    {
        var stockOut = Math.Max(stockIn - rawMaterial.CurrentStock, 0);
        var stockValue = rawMaterial.CurrentStock * rawMaterial.Cost;

        return new InventoryItemDto
        {
            RawMaterialId = rawMaterial.RawMaterialId,
            MaterialCode = rawMaterial.MaterialCode,
            MaterialName = rawMaterial.MaterialName,
            Unit = rawMaterial.Unit,
            CurrentStock = rawMaterial.CurrentStock,
            StockIn = stockIn,
            StockOut = stockOut,
            StockValue = stockValue,
            ReorderLevel = rawMaterial.ReorderLevel,
            IsBelowReorderLevel = rawMaterial.CurrentStock <= rawMaterial.ReorderLevel
        };
    }
}
