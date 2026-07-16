using System.ComponentModel.DataAnnotations;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.PurchaseOrders;

public class UpdatePurchaseOrderRequest
{
    [Required]
    [MaxLength(30)]
    public string PONumber { get; set; } = string.Empty;

    [Required]
    public Guid SupplierId { get; set; }

    public DateTime OrderDate { get; set; }

    public PurchaseOrderStatus Status { get; set; }

    [MinLength(1)]
    public List<UpdatePurchaseOrderItemRequest> Items { get; set; } = [];
}
