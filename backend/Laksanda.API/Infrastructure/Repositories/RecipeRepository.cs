using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Data;
using Laksanda.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Laksanda.API.Infrastructure.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RecipeRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Recipe>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Recipes
            .AsNoTracking()
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .OrderBy(x => x.RecipeCode)
            .ToListAsync(cancellationToken);
    }

    public Task<Recipe?> GetByIdAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Recipes
            .Include(x => x.Items)
                .ThenInclude(x => x.RawMaterial)
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId, cancellationToken);
    }

    public Task<Recipe?> GetByIdForUpdateAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.Recipes
            .FirstOrDefaultAsync(x => x.RecipeId == recipeId, cancellationToken);
    }

    public Task<Recipe?> GetByCodeAsync(string recipeCode, CancellationToken cancellationToken = default)
    {
        return _dbContext.Recipes
            .FirstOrDefaultAsync(x => x.RecipeCode == recipeCode, cancellationToken);
    }

    public async Task AddAsync(Recipe recipe, CancellationToken cancellationToken = default)
    {
        await _dbContext.Recipes.AddAsync(recipe, cancellationToken);
    }

    public Task DeleteItemsByRecipeIdAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        return _dbContext.RecipeItems
            .Where(x => x.RecipeId == recipeId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public Task AddItemsAsync(IEnumerable<RecipeItem> items, CancellationToken cancellationToken = default)
    {
        return _dbContext.RecipeItems.AddRangeAsync(items, cancellationToken);
    }

    public void Update(Recipe recipe)
    {
        _dbContext.Recipes.Update(recipe);
    }

    public void Delete(Recipe recipe)
    {
        _dbContext.Recipes.Remove(recipe);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}
