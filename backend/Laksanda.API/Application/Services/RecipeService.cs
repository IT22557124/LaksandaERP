using AutoMapper;
using FluentValidation;
using Laksanda.API.Application.DTOs.Recipes;
using Laksanda.API.Application.Interfaces.Repositories;
using Laksanda.API.Application.Interfaces.Services;
using Laksanda.API.Domain.Entities;

namespace Laksanda.API.Application.Services;

public class RecipeService : IRecipeService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRawMaterialRepository _rawMaterialRepository;
    private readonly IValidator<CreateRecipeRequest> _createValidator;
    private readonly IValidator<UpdateRecipeRequest> _updateValidator;
    private readonly IMapper _mapper;

    public RecipeService(
        IRecipeRepository recipeRepository,
        IRawMaterialRepository rawMaterialRepository,
        IValidator<CreateRecipeRequest> createValidator,
        IValidator<UpdateRecipeRequest> updateValidator,
        IMapper mapper)
    {
        _recipeRepository = recipeRepository;
        _rawMaterialRepository = rawMaterialRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RecipeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var recipes = await _recipeRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<RecipeDto>>(recipes);
    }

    public async Task<RecipeDto?> GetByIdAsync(Guid recipeId, CancellationToken cancellationToken = default)
    {
        var recipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken);
        return recipe is null ? null : _mapper.Map<RecipeDto>(recipe);
    }

    public async Task<RecipeDto> CreateAsync(CreateRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage).Distinct()));
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

            var recipeItem = _mapper.Map<RecipeItem>(item);
            items.Add(recipeItem);
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

        return _mapper.Map<RecipeDto>(savedRecipe);
    }

    public async Task<RecipeDto?> UpdateAsync(Guid recipeId, UpdateRecipeRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(" | ", validationResult.Errors.Select(x => x.ErrorMessage).Distinct()));
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

            var recipeItem = _mapper.Map<RecipeItem>(item);
            recipeItem.RecipeId = recipeId;
            newItems.Add(recipeItem);
        }

        recipe.RecipeCode = normalizedCode;
        recipe.RecipeName = request.RecipeName.Trim();
        recipe.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

        await _recipeRepository.DeleteItemsByRecipeIdAsync(recipeId, cancellationToken);
        await _recipeRepository.AddItemsAsync(newItems, cancellationToken);
        await _recipeRepository.SaveChangesAsync(cancellationToken);

        var savedRecipe = await _recipeRepository.GetByIdAsync(recipeId, cancellationToken)
            ?? recipe;

        return _mapper.Map<RecipeDto>(savedRecipe);
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
}
