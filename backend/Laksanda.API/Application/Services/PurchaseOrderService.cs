using Laksanda.API.Application.DTOs.PurchaseOrders;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IRawMaterialRepository _rawMaterialRepository;

    public PurchaseOrderService(
        IPurchaseOrderRepository purchaseOrderRepository,
        ISupplierRepository supplierRepository,
        IRawMaterialRepository rawMaterialRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _supplierRepository = supplierRepository;
        _rawMaterialRepository = rawMaterialRepository;
    }

    public async Task<IReadOnlyList<PurchaseOrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var purchaseOrders = await _purchaseOrderRepository.GetAllAsync(cancellationToken);
        return purchaseOrders.Select(MapToDto).ToArray();
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken);
        return purchaseOrder is null ? null : MapToDto(purchaseOrder);
    }

    public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var errors = PurchaseOrderValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedPONumber = request.PONumber.Trim();
        var existingByNumber = await _purchaseOrderRepository.GetByPONumberAsync(normalizedPONumber, cancellationToken);
        if (existingByNumber is not null)
        {
            throw new ArgumentException("PO Number already exists.");
        }

        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);
        if (supplier is null)
        {
            throw new ArgumentException("Supplier not found.");
        }

        var items = new List<PurchaseOrderItem>();
        foreach (var item in request.Items)
        {
            var material = await _rawMaterialRepository.GetByIdAsync(item.RawMaterialId, cancellationToken);
            if (material is null)
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' not found.");
            }

            items.Add(new PurchaseOrderItem
            {
                RawMaterialId = item.RawMaterialId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }

        var purchaseOrder = new PurchaseOrder
        {
            PONumber = normalizedPONumber,
            SupplierId = request.SupplierId,
            OrderDate = request.OrderDate,
            Status = request.Status,
            Items = items
        };

        await _purchaseOrderRepository.AddAsync(purchaseOrder, cancellationToken);
        await _purchaseOrderRepository.SaveChangesAsync(cancellationToken);

        var savedPurchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrder.PurchaseOrderId, cancellationToken)
            ?? purchaseOrder;

        return MapToDto(savedPurchaseOrder);
    }

    public async Task<PurchaseOrderDto?> UpdateAsync(Guid purchaseOrderId, UpdatePurchaseOrderRequest request, CancellationToken cancellationToken = default)
    {
        var errors = PurchaseOrderValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var purchaseOrder = await _purchaseOrderRepository.GetByIdForUpdateAsync(purchaseOrderId, cancellationToken);
        if (purchaseOrder is null)
        {
            return null;
        }

        var normalizedPONumber = request.PONumber.Trim();
        var existingByNumber = await _purchaseOrderRepository.GetByPONumberAsync(normalizedPONumber, cancellationToken);
        if (existingByNumber is not null && existingByNumber.PurchaseOrderId != purchaseOrderId)
        {
            throw new ArgumentException("PO Number already exists.");
        }

        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);
        if (supplier is null)
        {
            throw new ArgumentException("Supplier not found.");
        }

        var newItems = new List<PurchaseOrderItem>();
        foreach (var item in request.Items)
        {
            var material = await _rawMaterialRepository.GetByIdAsync(item.RawMaterialId, cancellationToken);
            if (material is null)
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' not found.");
            }

            newItems.Add(new PurchaseOrderItem
            {
                RawMaterialId = item.RawMaterialId,
                Quantity = item.Quantity,
                Price = item.Price
            });
        }

        purchaseOrder.PONumber = normalizedPONumber;
        purchaseOrder.SupplierId = request.SupplierId;
        purchaseOrder.OrderDate = request.OrderDate;
        purchaseOrder.Status = request.Status;

        await _purchaseOrderRepository.DeleteItemsByPurchaseOrderIdAsync(purchaseOrderId, cancellationToken);

        foreach (var item in newItems)
        {
            item.PurchaseOrderId = purchaseOrderId;
        }

        await _purchaseOrderRepository.AddItemsAsync(newItems, cancellationToken);

        await _purchaseOrderRepository.SaveChangesAsync(cancellationToken);

        var savedPurchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken)
            ?? purchaseOrder;

        return MapToDto(savedPurchaseOrder);
    }

    public async Task<bool> DeleteAsync(Guid purchaseOrderId, CancellationToken cancellationToken = default)
    {
        var purchaseOrder = await _purchaseOrderRepository.GetByIdAsync(purchaseOrderId, cancellationToken);
        if (purchaseOrder is null)
        {
            return false;
        }

        _purchaseOrderRepository.Delete(purchaseOrder);
        await _purchaseOrderRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static PurchaseOrderDto MapToDto(PurchaseOrder purchaseOrder)
    {
        return new PurchaseOrderDto
        {
            PurchaseOrderId = purchaseOrder.PurchaseOrderId,
            PONumber = purchaseOrder.PONumber,
            SupplierId = purchaseOrder.SupplierId,
            SupplierCode = purchaseOrder.Supplier?.SupplierCode ?? string.Empty,
            SupplierName = purchaseOrder.Supplier?.SupplierName ?? string.Empty,
            OrderDate = purchaseOrder.OrderDate,
            Status = purchaseOrder.Status,
            Items = purchaseOrder.Items.Select(item => new PurchaseOrderItemDto
            {
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                RawMaterialId = item.RawMaterialId,
                RawMaterialCode = item.RawMaterial?.MaterialCode ?? string.Empty,
                RawMaterialName = item.RawMaterial?.MaterialName ?? string.Empty,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToArray()
        };
    }
}
