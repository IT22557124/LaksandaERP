using System.Text.RegularExpressions;
using Laksanda.API.Application.DTOs.Products;

namespace Laksanda.API.Application.Validation;

public static partial class ProductValidation
{
    private static readonly Regex BarcodeRegex = BuildBarcodeRegex();

    public static IReadOnlyList<string> ValidateForCreate(CreateProductRequest request)
    {
        return Validate(request.ProductCode, request.ProductName, request.Category, request.Unit, request.SellingPrice, request.Barcode);
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdateProductRequest request)
    {
        return Validate(request.ProductCode, request.ProductName, request.Category, request.Unit, request.SellingPrice, request.Barcode);
    }

    private static IReadOnlyList<string> Validate(string code, string name, string? category, string? unit, decimal sellingPrice, string? barcode)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
        {
            errors.Add("ProductCode is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("ProductName is required.");
        }

        if (!string.IsNullOrWhiteSpace(category) && category.Length > 100)
        {
            errors.Add("Category must be 100 characters or less.");
        }

        if (!string.IsNullOrWhiteSpace(unit) && unit.Length > 50)
        {
            errors.Add("Unit must be 50 characters or less.");
        }

        if (sellingPrice < 0)
        {
            errors.Add("SellingPrice must be zero or greater.");
        }

        if (!string.IsNullOrWhiteSpace(barcode) && !BarcodeRegex.IsMatch(barcode))
        {
            errors.Add("Barcode format is invalid.");
        }

        return errors;
    }

    [GeneratedRegex("^[A-Za-z0-9\\-_.]+$")]
    private static partial Regex BuildBarcodeRegex();
}
