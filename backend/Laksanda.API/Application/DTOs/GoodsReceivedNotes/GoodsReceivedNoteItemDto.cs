namespace Laksanda.API.Application.DTOs.GoodsReceivedNotes;

public class GoodsReceivedNoteItemDto
{
    public Guid GoodsReceivedNoteItemId { get; set; }

    public Guid RawMaterialId { get; set; }

    public string RawMaterialCode { get; set; } = string.Empty;

    public string RawMaterialName { get; set; } = string.Empty;

    public decimal ReceivedQuantity { get; set; }

    public decimal UnitCost { get; set; }
}
