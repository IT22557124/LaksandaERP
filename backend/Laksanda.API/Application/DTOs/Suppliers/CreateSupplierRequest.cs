using System.ComponentModel.DataAnnotations;
using Laksanda.API.Domain.Enums;

namespace Laksanda.API.Application.DTOs.Suppliers;

public class CreateSupplierRequest
{
    [Required]
    [MaxLength(20)]
    public string SupplierCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [EmailAddress]
    [MaxLength(254)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public SupplierStatus Status { get; set; } = SupplierStatus.Active;
}
