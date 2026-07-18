namespace Laksanda.API.Application.DTOs.Inventory;

public class InventoryItemDto
{
    public Guid RawMaterialId { get; set; }

    public string MaterialCode { get; set; } = string.Empty;

    public string MaterialName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal CurrentStock { get; set; }

    public decimal StockIn { get; set; }

    public decimal StockOut { get; set; }

    public decimal StockValue { get; set; }

    public decimal ReorderLevel { get; set; }

    public bool IsBelowReorderLevel { get; set; }
}
