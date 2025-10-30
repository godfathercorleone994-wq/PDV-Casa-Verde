using Microsoft.AspNetCore.Mvc;
using PDVCasaVerde.DTOs;
using PDVCasaVerde.Models;
using PDVCasaVerde.Services;

namespace PDVCasaVerde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        var dtos = customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            Phone = c.Phone,
            Email = c.Email,
            Address = c.Address,
            Balance = c.Balance,
            IsActive = c.IsActive
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound(new { message = "Cliente não encontrado" });

        var dto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            Balance = customer.Balance,
            IsActive = customer.IsActive
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address
        };

        var created = await _customerService.CreateAsync(customer);
        var resultDto = new CustomerDto
        {
            Id = created.Id,
            Name = created.Name,
            Phone = created.Phone,
            Email = created.Email,
            Address = created.Address,
            Balance = created.Balance,
            IsActive = created.IsActive
        };

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            IsActive = dto.IsActive
        };

        var updated = await _customerService.UpdateAsync(id, customer);
        if (updated == null)
            return NotFound(new { message = "Cliente não encontrado" });

        var resultDto = new CustomerDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Phone = updated.Phone,
            Email = updated.Email,
            Address = updated.Address,
            Balance = updated.Balance,
            IsActive = updated.IsActive
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _customerService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Cliente não encontrado" });

        return NoContent();
    }

    [HttpPost("{id}/credit")]
    public async Task<ActionResult<CustomerDto>> AddCredit(int id, [FromBody] AddCreditDto dto)
    {
        var entry = await _customerService.AddCreditAsync(id, dto.Amount, dto.Description);
        if (entry == null)
            return NotFound(new { message = "Cliente não encontrado" });

        var customer = await _customerService.GetByIdAsync(id);
        var resultDto = new CustomerDto
        {
            Id = customer!.Id,
            Name = customer.Name,
            Phone = customer.Phone,
            Email = customer.Email,
            Address = customer.Address,
            Balance = customer.Balance,
            IsActive = customer.IsActive
        };

        return Ok(resultDto);
    }

    [HttpGet("{id}/ledger")]
    public async Task<ActionResult<List<CustomerLedgerDto>>> GetLedger(int id)
    {
        var entries = await _customerService.GetLedgerEntriesAsync(id);
        var dtos = entries.Select(e => new CustomerLedgerDto
        {
            Id = e.Id,
            Type = e.Type,
            Amount = e.Amount,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            SaleId = e.SaleId
        }).ToList();

        return Ok(dtos);
    }
}
