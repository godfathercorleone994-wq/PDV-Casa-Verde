using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class ProductService
{
    private readonly PDVContext _context;

    public ProductService(PDVContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByCodeAsync(int code)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive)
            .OrderBy(p => p.Code)
            .ToListAsync();
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.Code == product.Code);
        if (existing != null)
        {
            throw new InvalidOperationException($"Já existe um produto com o código {product.Code}");
        }
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);
        if (existing == null) return null;

        existing.Code = product.Code;
        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.IsActive = product.IsActive;

        await _context.SaveChangesAsync();
        return existing;
    }
}
