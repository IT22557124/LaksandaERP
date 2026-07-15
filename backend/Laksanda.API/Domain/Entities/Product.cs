using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Domain.Entities;

public class Product
{
    public Guid ProductId { get; set; } = Guid.NewGuid();

    public string ProductCode { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string? Category { get; set; }

    public string? Unit { get; set; }

    public decimal SellingPrice { get; set; }

    public string? Barcode { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
