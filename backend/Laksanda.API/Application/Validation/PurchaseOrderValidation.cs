using Laksanda.API.Application.DTOs.PurchaseOrders;

namespace Laksanda.API.Application.Validation;

public static class PurchaseOrderValidation
{
    public static IReadOnlyList<string> ValidateForCreate(CreatePurchaseOrderRequest request)
    {
        return Validate(request.PONumber, request.SupplierId, request.Items.Select(x => (x.RawMaterialId, x.Quantity, x.Price)).ToArray());
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdatePurchaseOrderRequest request)
    {
        return Validate(request.PONumber, request.SupplierId, request.Items.Select(x => (x.RawMaterialId, x.Quantity, x.Price)).ToArray());
    }

    private static IReadOnlyList<string> Validate(
        string poNumber,
        Guid supplierId,
        IReadOnlyCollection<(Guid RawMaterialId, decimal Quantity, decimal Price)> items)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(poNumber))
        {
            errors.Add("PONumber is required.");
        }

        if (supplierId == Guid.Empty)
        {
            errors.Add("Supplier is required.");
        }

        if (items.Count == 0)
        {
            errors.Add("At least one purchase order item is required.");
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

            if (item.Price < 0)
            {
                errors.Add("Item price must be zero or greater.");
            }
        }

        return errors;
    }
}
