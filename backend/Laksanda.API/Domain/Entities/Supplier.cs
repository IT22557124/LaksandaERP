using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Domain.Entities;

public class Supplier
{
    public Guid SupplierId { get; set; } = Guid.NewGuid();

    public string SupplierCode { get; set; } = string.Empty;

    public string SupplierName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public SupplierStatus Status { get; set; } = SupplierStatus.Active;
}
