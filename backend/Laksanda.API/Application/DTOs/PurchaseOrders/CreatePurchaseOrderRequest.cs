using System.ComponentModel.DataAnnotations;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.PurchaseOrders;

public class CreatePurchaseOrderRequest
{
    [Required]
    [MaxLength(30)]
    public string PONumber { get; set; } = string.Empty;

    [Required]
    public Guid SupplierId { get; set; }

    public DateTime OrderDate { get; set; }

    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;

    [MinLength(1)]
    public List<CreatePurchaseOrderItemRequest> Items { get; set; } = [];
}
