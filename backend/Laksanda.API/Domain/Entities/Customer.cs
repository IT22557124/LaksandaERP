namespace Laksanda.API.Domain.Entities;

public class Customer
{
    public Guid CustomerId { get; set; } = Guid.NewGuid();

    public string CustomerCode { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public decimal CreditLimit { get; set; }
}
