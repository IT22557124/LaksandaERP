namespace Laksanda.API.Domain.Entities;

public class RecipeItem
{
    public Guid RecipeItemId { get; set; } = Guid.NewGuid();

    public Guid RecipeId { get; set; }

    public Recipe? Recipe { get; set; }

    public Guid RawMaterialId { get; set; }

    public RawMaterial? RawMaterial { get; set; }

    public decimal Quantity { get; set; }
}
