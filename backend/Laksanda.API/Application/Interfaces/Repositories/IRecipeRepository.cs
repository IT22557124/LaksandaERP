using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Interfaces.Repositories;

public interface IRecipeRepository
{
    Task<IReadOnlyList<Recipe>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Recipe?> GetByIdAsync(Guid recipeId, CancellationToken cancellationToken = default);

    Task<Recipe?> GetByIdForUpdateAsync(Guid recipeId, CancellationToken cancellationToken = default);

    Task<Recipe?> GetByCodeAsync(string recipeCode, CancellationToken cancellationToken = default);

    Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default);

    Task DeleteItemsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken = default);

    Task AddItemsAsync(IEnumerable<RecipeItem> items, CancellationToken cancellationToken = default);

    void Update(Recipe recipe);

    void Delete(Recipe recipe);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
