using Laksanda.API.Application.DTOs.RawMaterials;

namespace Laksanda.API.Application.Validation;

public static class RawMaterialValidation
{
    public static IReadOnlyList<string> ValidateForCreate(CreateRawMaterialRequest request)
    {
        return Validate(request.MaterialCode, request.MaterialName, request.Unit, request.CurrentStock, request.ReorderLevel, request.Cost);
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdateRawMaterialRequest request)
    {
        return Validate(request.MaterialCode, request.MaterialName, request.Unit, request.CurrentStock, request.ReorderLevel, request.Cost);
    }

    private static IReadOnlyList<string> Validate(
        string materialCode,
        string materialName,
        string? unit,
        decimal currentStock,
        decimal reorderLevel,
        decimal cost)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(materialCode))
        {
            errors.Add("MaterialCode is required.");
        }

        if (string.IsNullOrWhiteSpace(materialName))
        {
            errors.Add("MaterialName is required.");
        }

        if (!string.IsNullOrWhiteSpace(unit) && unit.Length > 50)
        {
            errors.Add("Unit must be 50 characters or less.");
        }

        if (currentStock < 0)
        {
            errors.Add("CurrentStock must be zero or greater.");
        }

        if (reorderLevel < 0)
        {
            errors.Add("ReorderLevel must be zero or greater.");
        }

        if (cost < 0)
        {
            errors.Add("Cost must be zero or greater.");
        }

        return errors;
    }
}
