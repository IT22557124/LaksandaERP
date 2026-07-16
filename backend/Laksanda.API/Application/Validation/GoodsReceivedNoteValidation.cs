using Laksanda.API.Application.DTOs.GoodsReceivedNotes;

namespace Laksanda.API.Application.Validation;

public static class GoodsReceivedNoteValidation
{
    public static IReadOnlyList<string> ValidateForCreate(CreateGoodsReceivedNoteRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.GRNNumber))
        {
            errors.Add("GRNNumber is required.");
        }

        if (request.PurchaseOrderId == Guid.Empty)
        {
            errors.Add("Purchase order is required.");
        }

        if (request.Items.Count == 0)
        {
            errors.Add("At least one GRN item is required.");
        }

        var duplicateMaterialIds = request.Items
            .Where(x => x.RawMaterialId != Guid.Empty)
            .GroupBy(x => x.RawMaterialId)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .ToArray();

        if (duplicateMaterialIds.Length > 0)
        {
            errors.Add("Duplicate raw material items are not allowed in a GRN.");
        }

        foreach (var item in request.Items)
        {
            if (item.RawMaterialId == Guid.Empty)
            {
                errors.Add("Item material is required.");
            }

            if (item.ReceivedQuantity <= 0)
            {
                errors.Add("Item received quantity must be greater than zero.");
            }

            if (item.UnitCost < 0)
            {
                errors.Add("Item unit cost must be zero or greater.");
            }
        }

        return errors;
    }
}
