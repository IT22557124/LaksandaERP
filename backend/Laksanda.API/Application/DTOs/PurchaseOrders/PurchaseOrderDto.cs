using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.PurchaseOrders;

public class PurchaseOrderDto
{
    public Guid PurchaseOrderId { get; set; }

    public string PONumber { get; set; } = string.Empty;

    public Guid SupplierId { get; set; }

    public string SupplierCode { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public DateTime OrderDate { get; set; }

    public PurchaseOrderStatus Status { get; set; }

    public IReadOnlyCollection<PurchaseOrderItemDto> Items { get; set; } = Array.Empty<PurchaseOrderItemDto>();
}
