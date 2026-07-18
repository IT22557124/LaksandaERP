namespace Laksanda.API.Domain.Entities;

public class Recipe
{
    public Guid RecipeId { get; set; } = Guid.NewGuid();

    public string RecipeCode { get; set; } = string.Empty;

    public string RecipeName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<RecipeItem> Items { get; set; } = new List<RecipeItem>();
}
