namespace Laksanda.API.Application.DTOs.Recipes;

public class RecipeItemDto
{
    public Guid RecipeItemId { get; set; }

    public Guid RawMaterialId { get; set; }

    public string RawMaterialCode { get; set; } = string.Empty;

    public string RawMaterialName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal Quantity { get; set; }
}
