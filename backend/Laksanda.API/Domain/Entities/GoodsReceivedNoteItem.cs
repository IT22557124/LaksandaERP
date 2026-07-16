namespace Laksanda.API.Domain.Entities;

public class GoodsReceivedNoteItem
{
    public Guid GoodsReceivedNoteItemId { get; set; } = Guid.NewGuid();

    public Guid GoodsReceivedNoteId { get; set; }

    public GoodsReceivedNote? GoodsReceivedNote { get; set; }

    public Guid RawMaterialId { get; set; }

    public RawMaterial? RawMaterial { get; set; }

    public decimal ReceivedQuantity { get; set; }

    public decimal UnitCost { get; set; }
}
