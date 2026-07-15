using System.ComponentModel.DataAnnotations;

namespace Laksanda.API.Application.DTOs.Customers;

public class CreateCustomerRequest
{
    [Required]
    [MaxLength(20)]
    public string CustomerCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    [EmailAddress]
    [MaxLength(254)]
    public string? Email { get; set; }

    [Range(0, double.MaxValue)]
    public decimal CreditLimit { get; set; }
}
