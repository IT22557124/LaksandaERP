using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.GoodsReceivedNotes;

public class CreateGoodsReceivedNoteRequest
{
    [Required]
    [MaxLength(30)]
    public string GRNNumber { get; set; } = string.Empty;

    [Required]
    public Guid PurchaseOrderId { get; set; }

    public DateTime ReceivedDate { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }

    [MinLength(1)]
    public List<CreateGoodsReceivedNoteItemRequest> Items { get; set; } = [];
}
