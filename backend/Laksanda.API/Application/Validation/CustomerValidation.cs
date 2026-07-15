using System.Net.Mail;
using System.Text.RegularExpressions;
using Laksanda.API.Application.DTOs.Customers;

namespace Laksanda.API.Application.Validation;

public static partial class CustomerValidation
{
    private static readonly Regex PhoneRegex = BuildPhoneRegex();

    public static IReadOnlyList<string> ValidateForCreate(CreateCustomerRequest request)
    {
        return Validate(request.CustomerCode, request.CustomerName, request.Phone, request.Email, request.Address, request.CreditLimit);
    }

    public static IReadOnlyList<string> ValidateForUpdate(UpdateCustomerRequest request)
    {
        return Validate(request.CustomerCode, request.CustomerName, request.Phone, request.Email, request.Address, request.CreditLimit);
    }

    private static IReadOnlyList<string> Validate(string code, string name, string? phone, string? email, string? address, decimal creditLimit)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(code))
        {
            errors.Add("CustomerCode is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("CustomerName is required.");
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

        if (creditLimit < 0)
        {
            errors.Add("CreditLimit must be zero or greater.");
        }

        return errors;
    }

    [GeneratedRegex("^[0-9+()\\-\\s]{7,30}$")]
    private static partial Regex BuildPhoneRegex();
}
