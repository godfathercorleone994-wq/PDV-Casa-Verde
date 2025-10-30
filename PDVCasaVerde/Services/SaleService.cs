using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class SaleService
{
    private readonly PDVContext _context;
    private readonly CustomerService _customerService;

    public SaleService(PDVContext context, CustomerService customerService)
    {
        _context = context;
        _customerService = customerService;
    }

    public async Task<Sale> CreateSaleAsync(int? tableNumber, int? customerId)
    {
        var lastSaleNumber = await _context.Sales
            .OrderByDescending(s => s.SaleNumber)
            .Select(s => s.SaleNumber)
            .FirstOrDefaultAsync();

        var sale = new Sale
        {
            SaleNumber = lastSaleNumber + 1,
            TableNumber = tableNumber,
            CustomerId = customerId,
            OpenedAt = DateTime.UtcNow,
            IsOpen = true,
            Status = "OPEN"
        };

        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task<Sale?> GetSaleAsync(int saleId)
    {
        return await _context.Sales
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == saleId);
    }

    public async Task<Sale?> GetSaleByNumberAsync(int saleNumber)
    {
        return await _context.Sales
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber);
    }

    public async Task<List<Sale>> GetOpenSalesAsync()
    {
        return await _context.Sales
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Include(s => s.Customer)
            .Where(s => s.IsOpen)
            .OrderBy(s => s.OpenedAt)
            .ToListAsync();
    }

    public async Task<List<Sale>> GetSalesByTableAsync(int tableNumber)
    {
        return await _context.Sales
            .Include(s => s.Items)
                .ThenInclude(i => i.Product)
            .Where(s => s.TableNumber == tableNumber && s.IsOpen)
            .OrderBy(s => s.OpenedAt)
            .ToListAsync();
    }

    public async Task<bool> AddItemToSaleAsync(int saleId, int productCode, int quantity)
    {
        var sale = await _context.Sales.FindAsync(saleId);
        if (sale == null || !sale.IsOpen)
            return false;

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == productCode);
        if (product == null)
            return false;

        var item = new SaleItem
        {
            SaleId = saleId,
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * quantity,
            AddedAt = DateTime.UtcNow
        };

        sale.TotalAmount += item.TotalPrice;

        _context.SaleItems.Add(item);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Sale?> CloseSaleAsync(int saleId, string paymentType)
    {
        var sale = await _context.Sales
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == saleId);

        if (sale == null || !sale.IsOpen)
            return null;

        sale.IsOpen = false;
        sale.Status = "CLOSED";
        sale.ClosedAt = DateTime.UtcNow;
        sale.PaymentType = paymentType;

        // If payment type is LEDGER, add debt to customer
        if (paymentType == "LEDGER" && sale.CustomerId.HasValue)
        {
            await _customerService.AddDebtAsync(sale.CustomerId.Value, sale.Id, sale.TotalAmount);
        }

        await _context.SaveChangesAsync();
        return sale;
    }

    public async Task<bool> CancelSaleAsync(int saleId)
    {
        var sale = await _context.Sales.FindAsync(saleId);
        if (sale == null || !sale.IsOpen)
            return false;

        sale.IsOpen = false;
        sale.Status = "CANCELLED";
        sale.ClosedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}
