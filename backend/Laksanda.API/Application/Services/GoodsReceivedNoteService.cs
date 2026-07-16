using Laksanda.API.Application.DTOs.GoodsReceivedNotes;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class GoodsReceivedNoteService : IGoodsReceivedNoteService
{
    private readonly IGoodsReceivedNoteRepository _goodsReceivedNoteRepository;
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IRawMaterialRepository _rawMaterialRepository;
    private readonly ApplicationDbContext _dbContext;

    public GoodsReceivedNoteService(
        IGoodsReceivedNoteRepository goodsReceivedNoteRepository,
        IPurchaseOrderRepository purchaseOrderRepository,
        IRawMaterialRepository rawMaterialRepository,
        ApplicationDbContext dbContext)
    {
        _goodsReceivedNoteRepository = goodsReceivedNoteRepository;
        _purchaseOrderRepository = purchaseOrderRepository;
        _rawMaterialRepository = rawMaterialRepository;
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<GoodsReceivedNoteDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var notes = await _goodsReceivedNoteRepository.GetAllAsync(cancellationToken);
        return notes.Select(MapToDto).ToArray();
    }

    public async Task<GoodsReceivedNoteDto?> GetByIdAsync(Guid goodsReceivedNoteId, CancellationToken cancellationToken = default)
    {
        var note = await _goodsReceivedNoteRepository.GetByIdAsync(goodsReceivedNoteId, cancellationToken);
        return note is null ? null : MapToDto(note);
    }

    public async Task<GoodsReceivedNoteDto> CreateAsync(CreateGoodsReceivedNoteRequest request, CancellationToken cancellationToken = default)
    {
        var errors = GoodsReceivedNoteValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedGRNNumber = request.GRNNumber.Trim();
        var existingByNumber = await _goodsReceivedNoteRepository.GetByGRNNumberAsync(normalizedGRNNumber, cancellationToken);
        if (existingByNumber is not null)
        {
            throw new ArgumentException("GRN Number already exists.");
        }

        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(request.PurchaseOrderId, cancellationToken);
        if (purchaseOrder is null)
        {
            throw new ArgumentException("Purchase order not found.");
        }

        var purchaseOrderMaterialIds = purchaseOrder.Items
            .Select(x => x.RawMaterialId)
            .ToHashSet();

        var items = new List<GoodsReceivedNoteItem>();
        var materialsToUpdate = new List<RawMaterial>();

        foreach (var item in request.Items)
        {
            if (!purchaseOrderMaterialIds.Contains(item.RawMaterialId))
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' does not exist in the selected purchase order.");
            }

            var material = await _rawMaterialRepository.GetByIdAsync(item.RawMaterialId, cancellationToken);
            if (material is null)
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' not found.");
            }

            material.CurrentStock += item.ReceivedQuantity;

            items.Add(new GoodsReceivedNoteItem
            {
                RawMaterialId = item.RawMaterialId,
                ReceivedQuantity = item.ReceivedQuantity,
                UnitCost = item.UnitCost
            });

            materialsToUpdate.Add(material);
        }

        var note = new GoodsReceivedNote
        {
            GRNNumber = normalizedGRNNumber,
            PurchaseOrderId = request.PurchaseOrderId,
            ReceivedDate = request.ReceivedDate,
            Remarks = string.IsNullOrWhiteSpace(request.Remarks) ? null : request.Remarks.Trim(),
            Items = items
        };

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await _goodsReceivedNoteRepository.AddAsync(note, cancellationToken);

            foreach (var material in materialsToUpdate)
            {
                _rawMaterialRepository.Update(material);
            }

            await _goodsReceivedNoteRepository.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        var savedNote = await _goodsReceivedNoteRepository.GetByIdAsync(note.GoodsReceivedNoteId, cancellationToken)
            ?? note;

        return MapToDto(savedNote);
    }

    private static GoodsReceivedNoteDto MapToDto(GoodsReceivedNote note)
    {
        return new GoodsReceivedNoteDto
        {
            GoodsReceivedNoteId = note.GoodsReceivedNoteId,
            GRNNumber = note.GRNNumber,
            PurchaseOrderId = note.PurchaseOrderId,
            PONumber = note.PurchaseOrder?.PONumber ?? string.Empty,
            SupplierId = note.PurchaseOrder?.SupplierId ?? Guid.Empty,
            SupplierCode = note.PurchaseOrder?.Supplier?.SupplierCode ?? string.Empty,
            SupplierName = note.PurchaseOrder?.Supplier?.SupplierName ?? string.Empty,
            ReceivedDate = note.ReceivedDate,
            Remarks = note.Remarks,
            Items = note.Items.Select(item => new GoodsReceivedNoteItemDto
            {
                GoodsReceivedNoteItemId = item.GoodsReceivedNoteItemId,
                RawMaterialId = item.RawMaterialId,
                RawMaterialCode = item.RawMaterial?.MaterialCode ?? string.Empty,
                RawMaterialName = item.RawMaterial?.MaterialName ?? string.Empty,
                ReceivedQuantity = item.ReceivedQuantity,
                UnitCost = item.UnitCost
            }).ToArray()
        };
    }
}
