using System.ComponentModel.DataAnnotations;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.Products;

public class CreateProductRequest
{
    [Required]
    [MaxLength(20)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Category { get; set; }

    [MaxLength(50)]
    public string? Unit { get; set; }

    [Range(0, double.MaxValue)]
    public decimal SellingPrice { get; set; }

    [MaxLength(100)]
    public string? Barcode { get; set; }

    public ProductStatus Status { get; set; } = ProductStatus.Active;
}
