namespace Laksanda.API.Domain.Entities;

public class PurchaseOrderItem
{
    public Guid PurchaseOrderItemId { get; set; } = Guid.NewGuid();

    public Guid PurchaseOrderId { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }

    public Guid RawMaterialId { get; set; }

    public RawMaterial? RawMaterial { get; set; }

    public decimal Quantity { get; set; }

    public decimal Price { get; set; }
}
