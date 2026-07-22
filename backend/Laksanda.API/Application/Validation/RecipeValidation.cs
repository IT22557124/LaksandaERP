using FluentValidation;
using Laksanda.API.Application.DTOs.Recipes;

namespace Laksanda.API.Application.Validation;

public class CreateRecipeRequestValidator : AbstractValidator<CreateRecipeRequest>
{
    public CreateRecipeRequestValidator()
    {
        RuleFor(x => x.RecipeCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.RecipeName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(NotContainDuplicateRawMaterials)
            .WithMessage("Duplicate raw material items are not allowed in a recipe.")
            .Must(HaveTotalPercentageExactlyHundred)
            .WithMessage("Total recipe item percentage must equal exactly 100.");

        RuleForEach(x => x.Items)
            .SetValidator(new CreateRecipeItemRequestValidator());
    }

    private static bool NotContainDuplicateRawMaterials(IEnumerable<CreateRecipeItemRequest> items)
    {
        var duplicateMaterialIds = items
            .Where(x => x.RawMaterialId != Guid.Empty)
            .GroupBy(x => x.RawMaterialId)
            .Any(x => x.Count() > 1);

        return !duplicateMaterialIds;
    }

    private static bool HaveTotalPercentageExactlyHundred(IEnumerable<CreateRecipeItemRequest> items)
    {
        return items.Sum(x => x.Percentage) == 100m;
    }
}

public class UpdateRecipeRequestValidator : AbstractValidator<UpdateRecipeRequest>
{
    public UpdateRecipeRequestValidator()
    {
        RuleFor(x => x.RecipeCode)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.RecipeName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.Items)
            .NotEmpty()
            .Must(NotContainDuplicateRawMaterials)
            .WithMessage("Duplicate raw material items are not allowed in a recipe.")
            .Must(HaveTotalPercentageExactlyHundred)
            .WithMessage("Total recipe item percentage must equal exactly 100.");

        RuleForEach(x => x.Items)
            .SetValidator(new UpdateRecipeItemRequestValidator());
    }

    private static bool NotContainDuplicateRawMaterials(IEnumerable<UpdateRecipeItemRequest> items)
    {
        var duplicateMaterialIds = items
            .Where(x => x.RawMaterialId != Guid.Empty)
            .GroupBy(x => x.RawMaterialId)
            .Any(x => x.Count() > 1);

        return !duplicateMaterialIds;
    }

    private static bool HaveTotalPercentageExactlyHundred(IEnumerable<UpdateRecipeItemRequest> items)
    {
        return items.Sum(x => x.Percentage) == 100m;
    }
}

public class CreateRecipeItemRequestValidator : AbstractValidator<CreateRecipeItemRequest>
{
    public CreateRecipeItemRequestValidator()
    {
        RuleFor(x => x.RawMaterialId)
            .NotEmpty()
            .WithMessage("Item material is required.");

        RuleFor(x => x.Percentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Item quantity must be greater than zero.");

        RuleFor(x => x.Unit)
            .NotEmpty()
            .MaximumLength(50);
    }
}

public class UpdateRecipeItemRequestValidator : AbstractValidator<UpdateRecipeItemRequest>
{
    public UpdateRecipeItemRequestValidator()
    {
        RuleFor(x => x.RawMaterialId)
            .NotEmpty()
            .WithMessage("Item material is required.");

        RuleFor(x => x.Percentage)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Item quantity must be greater than zero.");

        RuleFor(x => x.Unit)
            .NotEmpty()
            .MaximumLength(50);
    }
}
