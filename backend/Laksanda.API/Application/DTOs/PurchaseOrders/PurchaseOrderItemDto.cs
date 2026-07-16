namespace Laksanda.API.Application.DTOs.PurchaseOrders;

public class PurchaseOrderItemDto
{
    public Guid PurchaseOrderItemId { get; set; }

    public Guid RawMaterialId { get; set; }

    public string RawMaterialCode { get; set; } = string.Empty;

    public string RawMaterialName { get; set; } = string.Empty;

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }
}
