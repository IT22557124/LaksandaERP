using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.Recipes;

public class CreateRecipeRequest
{
    [Required]
    [MaxLength(20)]
    public string RecipeCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string RecipeName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MinLength(1)]
    public List<CreateRecipeItemRequest> Items { get; set; } = [];
}
