namespace Laksanda.API.Domain.Entities;

public class GoodsReceivedNote
{
    public Guid GoodsReceivedNoteId { get; set; } = Guid.NewGuid();

    public string GRNNumber { get; set; } = string.Empty;

    public Guid PurchaseOrderId { get; set; }

    public PurchaseOrder? PurchaseOrder { get; set; }

    public DateTime ReceivedDate { get; set; }

    public string? Remarks { get; set; }

    public ICollection<GoodsReceivedNoteItem> Items { get; set; } = new List<GoodsReceivedNoteItem>();
}
