using System.ComponentModel.DataAnnotations;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.RawMaterials;

public class CreateRawMaterialRequest
{
    [Required]
    [MaxLength(20)]
    public string MaterialCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string MaterialName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CurrentStock { get; set; }

    [Range(0, double.MaxValue)]
    public decimal ReorderLevel { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Cost { get; set; }

    public RawMaterialStatus Status { get; set; } = RawMaterialStatus.Active;
}
