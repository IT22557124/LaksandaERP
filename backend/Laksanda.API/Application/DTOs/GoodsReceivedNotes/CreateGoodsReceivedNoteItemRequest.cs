using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.GoodsReceivedNotes;

public class CreateGoodsReceivedNoteItemRequest
{
    [Required]
    public Guid RawMaterialId { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
    public decimal ReceivedQuantity { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal UnitCost { get; set; }
}
