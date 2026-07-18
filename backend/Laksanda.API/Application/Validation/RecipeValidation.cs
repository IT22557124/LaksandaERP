using Laksanda.API.Application.DTOs.Recipes;

namespace Laksanda.API.Application.Validation;

public static class RecipeValidation
{
    public static IReadOnlyList<string> ValidateForCreate(CreateRecipeRequest request)
    {
        return Validate(
            request.RecipeCode,
            request.RecipeName,
            request.Description,
            request.Items.Select(x => (x.RawMaterialId, x.Quantity)).ToArray());
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdateRecipeRequest request)
    {
        return Validate(
            request.RecipeCode,
            request.RecipeName,
            request.Description,
            request.Items.Select(x => (x.RawMaterialId, x.Quantity)).ToArray());
    }

    private static IReadOnlyList<string> Validate(
        string recipeCode,
        string recipeName,
        string? description,
        IReadOnlyCollection<(Guid RawMaterialId, decimal Quantity)> items)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(recipeCode))
        {
            errors.Add("RecipeCode is required.");
        }

        if (string.IsNullOrWhiteSpace(recipeName))
        {
            errors.Add("RecipeName is required.");
        }

        if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
        {
            errors.Add("Description must be 500 characters or less.");
        }

        if (items.Count == 0)
        {
            errors.Add("At least one recipe item is required.");
        }

        var duplicateMaterialIds = items
            .Where(x => x.RawMaterialId != Guid.Empty)
            .GroupBy(x => x.RawMaterialId)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicateMaterialIds.Length > 0)
        {
            errors.Add("Duplicate raw material items are not allowed in a recipe.");
        }

        foreach (var item in items)
        {
            if (item.RawMaterialId == Guid.Empty)
            {
                errors.Add("Item material is required.");
            }

            if (item.Quantity <= 0)
            {
                errors.Add("Item quantity must be greater than zero.");
            }
        }

        return errors;
    }
}
