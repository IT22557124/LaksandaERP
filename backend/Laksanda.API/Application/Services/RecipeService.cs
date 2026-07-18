using Laksanda.API.Application.DTOs.Recipes;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Application.Validation;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRawMaterialRepository _rawMaterialRepository;

    public RecipeService(IRecipeRepository recipeRepository, IRawMaterialRepository rawMaterialRepository)
    {
        _recipeRepository = recipeRepository;
        _rawMaterialRepository = rawMaterialRepository;
    }

    public async Task<IReadOnlyList<RecipeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetAllAsync(cancellationToken);
        return recipes.Select(MapToDto).ToArray();
    }

    public async Task<RecipeDto?> GetByIdAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken);
        return recipe is null ? null : MapToDto(recipe);
    }

    public async Task<RecipeDto> CreateAsync(CreateRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var errors = RecipeValidation.ValidateForCreate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var normalizedCode = request.RecipeCode.Trim();
        var existingByCode = await _recipeRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null)
        {
            throw new ArgumentException("RecipeCode already exists.");
        }

        var items = new List<RecipeItem>();
        foreach (var item in request.Items)
        {
            var material = await _rawMaterialRepository.GetByIdAsync(item.RawMaterialId, cancellationToken);
            if (material is null)
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' not found.");
            }

            items.Add(new RecipeItem
            {
                RawMaterialId = item.RawMaterialId,
                Quantity = item.Quantity
            });
        }

        var recipe = new Recipe
        {
            RecipeCode = normalizedCode,
            RecipeName = request.RecipeName.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            Items = items
        };

        await _recipeRepository.AddAsync(recipe, cancellationToken);
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        var savedRecipe = await _recipeRepository.GetByIdAsync(recipe.RecipeId, cancellationToken)
            ?? recipe;

        return MapToDto(savedRecipe);
    }

    public async Task<RecipeDto?> UpdateAsync(Guid recipeId, UpdateRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var errors = RecipeValidation.ValidateForUpdate(request);
        if (errors.Count > 0)
        {
            throw new ArgumentException(string.Join(" | ", errors));
        }

        var recipe = await _recipeRepository.GetByIdForUpdateAsync(recipeId, cancellationToken);
        if (recipe is null)
        {
            return null;
        }

        var normalizedCode = request.RecipeCode.Trim();
        var existingByCode = await _recipeRepository.GetByCodeAsync(normalizedCode, cancellationToken);
        if (existingByCode is not null && existingByCode.RecipeId != recipeId)
        {
            throw new ArgumentException("RecipeCode already exists.");
        }

        var newItems = new List<RecipeItem>();
        foreach (var item in request.Items)
        {
            var material = await _rawMaterialRepository.GetByIdAsync(item.RawMaterialId, cancellationToken);
            if (material is null)
            {
                throw new ArgumentException($"Material '{item.RawMaterialId}' not found.");
            }

            newItems.Add(new RecipeItem
            {
                RecipeId = recipeId,
                RawMaterialId = item.RawMaterialId,
                Quantity = item.Quantity
            });
        }

        recipe.RecipeCode = normalizedCode;
        recipe.RecipeName = request.RecipeName.Trim();
        recipe.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

        await _recipeRepository.DeleteItemsByRecipeIdAsync(recipeId, cancellationToken);
        await _recipeRepository.AddItemsAsync(newItems, cancellationToken);
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        var savedRecipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken)
            ?? recipe;

        return MapToDto(savedRecipe);
    }

    public async Task<bool> DeleteAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _recipeRepository.GetByIdForUpdateAsync(recipeId, cancellationToken);
        if (recipe is null)
        {
            return false;
        }

        _recipeRepository.Delete(recipe);
        await _recipeRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static RecipeDto MapToDto(Recipe recipe)
    {
        return new RecipeDto
        {
            RecipeId = recipe.RecipeId,
            RecipeCode = recipe.RecipeCode,
            RecipeName = recipe.RecipeName,
            Description = recipe.Description,
            Items = recipe.Items.Select(item => new RecipeItemDto
            {
                RecipeItemId = item.RecipeItemId,
                RawMaterialId = item.RawMaterialId,
                RawMaterialCode = item.RawMaterial?.MaterialCode ?? string.Empty,
                RawMaterialName = item.RawMaterial?.MaterialName ?? string.Empty,
                Unit = item.RawMaterial?.Unit,
                Quantity = item.Quantity
            }).ToArray()
        };
    }
}
