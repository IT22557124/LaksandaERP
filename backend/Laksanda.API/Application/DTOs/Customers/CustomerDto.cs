namespace Laksanda.API.Application.DTOs.Customers;

public class CustomerDto
{
    public Guid CustomerId { get; set; }

    public string CustomerCode { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public decimal CreditLimit { get; set; }
}
