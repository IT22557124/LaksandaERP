using Laksanda.API.Application.DTOs.Recipes;

namespace Laksanda.API.Application.Interfaces.Services;

public interface IRecipeService
{
    Task<IReadOnlyList<RecipeDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<RecipeDto?> GetByIdAsync(Guid recipeId, CancellationToken cancellationToken = default);

    Task<RecipeDto> CreateAsync(CreateRecipeRequest request, CancellationToken cancellationToken = default);

    Task<RecipeDto?> UpdateAsync(Guid recipeId, UpdateRecipeRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid recipeId, CancellationToken cancellationToken = default);
}
