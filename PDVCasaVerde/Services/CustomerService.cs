using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class CustomerService
{
    private readonly PDVContext _context;

    public CustomerService(PDVContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers
            .Include(c => c.LedgerEntries.OrderByDescending(e => e.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        customer.Balance = 0;
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer?> UpdateAsync(int id, Customer customer)
    {
        var existingCustomer = await _context.Customers.FindAsync(id);
        if (existingCustomer == null)
            return null;

        existingCustomer.Name = customer.Name;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.Email = customer.Email;
        existingCustomer.Address = customer.Address;
        existingCustomer.IsActive = customer.IsActive;

        await _context.SaveChangesAsync();
        return existingCustomer;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
            return false;

        customer.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<CustomerLedgerEntry?> AddCreditAsync(int customerId, decimal amount, string? description)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            return null;

        var entry = new CustomerLedgerEntry
        {
            CustomerId = customerId,
            Type = "PAYMENT",
            Amount = amount,
            Description = description ?? "Pagamento em conta",
            CreatedAt = DateTime.UtcNow
        };

        customer.Balance += amount;
        _context.CustomerLedgerEntries.Add(entry);
        await _context.SaveChangesAsync();

        return entry;
    }

    public async Task<List<CustomerLedgerEntry>> GetLedgerEntriesAsync(int customerId)
    {
        return await _context.CustomerLedgerEntries
            .Where(e => e.CustomerId == customerId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    internal async Task<bool> AddDebtAsync(int customerId, int saleId, decimal amount)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
            return false;

        var entry = new CustomerLedgerEntry
        {
            CustomerId = customerId,
            Type = "SALE",
            Amount = -amount,
            Description = $"Venda #{saleId}",
            SaleId = saleId,
            CreatedAt = DateTime.UtcNow
        };

        customer.Balance -= amount;
        _context.CustomerLedgerEntries.Add(entry);
        await _context.SaveChangesAsync();

        return true;
    }
}
