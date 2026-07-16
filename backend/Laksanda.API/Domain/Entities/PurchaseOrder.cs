using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Domain.Entities;

public class PurchaseOrder
{
    public Guid PurchaseOrderId { get; set; } = Guid.NewGuid();

    public string PONumber { get; set; } = string.Empty;

    public Guid SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    public DateTime OrderDate { get; set; }

    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
