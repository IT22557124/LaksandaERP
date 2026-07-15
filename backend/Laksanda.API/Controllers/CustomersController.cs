using Laksanda.API.Application.DTOs.Customers;
using Laksanda.API.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laksanda.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _customerService.GetAllAsync(cancellationToken);
        return Ok(customers);
    }

    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _customerService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { customerId = created.CustomerId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{customerId:guid}")]
    public async Task<ActionResult<CustomerDto>> Update(Guid customerId, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _customerService.UpdateAsync(customerId, request, cancellationToken);
            if (updated is null)
            {
                return NotFound();
            }

            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{customerId:guid}")]
    public async Task<IActionResult> Delete(Guid customerId, CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(customerId, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
