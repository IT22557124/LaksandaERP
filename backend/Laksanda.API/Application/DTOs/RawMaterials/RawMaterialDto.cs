using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.RawMaterials;

public class RawMaterialDto
{
    public Guid RawMaterialId { get; set; }

    public string MaterialCode { get; set; } = string.Empty;

    public string MaterialName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal CurrentStock { get; set; }

    public decimal ReorderLevel { get; set; }

    public decimal Cost { get; set; }

    public RawMaterialStatus Status { get; set; }
}
