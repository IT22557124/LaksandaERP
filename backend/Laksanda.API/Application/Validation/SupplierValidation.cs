using System.Net.Mail;
using System.Text.RegularExpressions;
using Laksanda.API.Application.DTOs.Suppliers;

namespace Laksanda.API.Application.Validation;

public static partial class SupplierValidation
{
    private static readonly Regex PhoneRegex = BuildPhoneRegex();

    public static IReadOnlyList<string> ValidateForCreate(CreateSupplierRequest request)
    {
        return Validate(request.SupplierCode, request.SupplierName, request.Phone, request.Email, request.Address);
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdateSupplierRequest request)
    {
        return Validate(request.SupplierCode, request.SupplierName, request.Phone, request.Email, request.Address);
    }

    private static IReadOnlyList<string> Validate(string code, string name, string? phone, string? email, string? address)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
        {
            errors.Add("SupplierCode is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("SupplierName is required.");
        }

        if (!string.IsNullOrWhiteSpace(phone) && !PhoneRegex.IsMatch(phone))
        {
            errors.Add("Phone format is invalid.");
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            try
            {
                _ = new MailAddress(email);
            }
            catch
            {
                errors.Add("Email format is invalid.");
            }
        }

        if (!string.IsNullOrWhiteSpace(address) && address.Length > 500)
        {
            errors.Add("Address must be 500 characters or less.");
        }

        return errors;
    }

    [GeneratedRegex("^[0-9+()\\-\\s]{7,30}$")]
    private static partial Regex BuildPhoneRegex();
}
