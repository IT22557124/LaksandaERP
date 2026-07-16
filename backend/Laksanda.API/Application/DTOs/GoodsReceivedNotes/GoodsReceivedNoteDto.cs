namespace Laksanda.API.Application.DTOs.GoodsReceivedNotes;

public class GoodsReceivedNoteDto
{
    public Guid GoodsReceivedNoteId { get; set; }

    public string GRNNumber { get; set; } = string.Empty;

    public Guid PurchaseOrderId { get; set; }

    public string PONumber { get; set; } = string.Empty;

    public Guid SupplierId { get; set; }

    public string SupplierCode { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public DateTime ReceivedDate { get; set; }

    public string? Remarks { get; set; }

    public IReadOnlyCollection<GoodsReceivedNoteItemDto> Items { get; set; } = Array.Empty<GoodsReceivedNoteItemDto>();
}
