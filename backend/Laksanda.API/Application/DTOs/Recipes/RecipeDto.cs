namespace Laksanda.API.Application.DTOs.Recipes;

public class RecipeDto
{
    public Guid RecipeId { get; set; }

    public string RecipeCode { get; set; } = string.Empty;

    public string RecipeName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IReadOnlyCollection<RecipeItemDto> Items { get; set; } = Array.Empty<RecipeItemDto>();
}
