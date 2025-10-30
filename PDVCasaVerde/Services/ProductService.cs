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

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Group)
            .Include(p => p.Subgroup)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetByCodeAsync(int code)
    {
        return await _context.Products
            .Include(p => p.Group)
            .Include(p => p.Subgroup)
            .FirstOrDefaultAsync(p => p.Code == code && p.IsActive);
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Group)
            .Include(p => p.Subgroup)
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

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing == null) return null;

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Category = product.Category;
        existing.IsActive = product.IsActive;
        existing.GroupId = product.GroupId;
        existing.SubgroupId = product.SubgroupId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}

