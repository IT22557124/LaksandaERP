using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Domain.Entities;

public class RawMaterial
{
    public Guid RawMaterialId { get; set; } = Guid.NewGuid();

    public string MaterialCode { get; set; } = string.Empty;

    public string MaterialName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal CurrentStock { get; set; }

    public decimal ReorderLevel { get; set; }

    public decimal Cost { get; set; }

    public RawMaterialStatus Status { get; set; } = RawMaterialStatus.Active;
}
