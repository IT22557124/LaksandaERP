using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.PurchaseOrders;

public class CreatePurchaseOrderItemRequest
{
    [Required]
    public Guid RawMaterialId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}
