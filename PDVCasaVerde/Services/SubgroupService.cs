using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class SubgroupService
{
    private readonly PDVContext _context;

    public SubgroupService(PDVContext context)
    {
        _context = context;
    }

    public async Task<List<Subgroup>> GetAllAsync()
    {
        return await _context.Subgroups
            .Include(s => s.Group)
            .Where(s => s.IsActive)
            .ToListAsync();
    }

    public async Task<List<Subgroup>> GetByGroupIdAsync(int groupId)
    {
        return await _context.Subgroups
            .Include(s => s.Group)
            .Where(s => s.GroupId == groupId && s.IsActive)
            .ToListAsync();
    }

    public async Task<Subgroup?> GetByIdAsync(int id)
    {
        return await _context.Subgroups
            .Include(s => s.Group)
            .Include(s => s.Products)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Subgroup> CreateAsync(Subgroup subgroup)
    {
        subgroup.CreatedAt = DateTime.UtcNow;
        _context.Subgroups.Add(subgroup);
        await _context.SaveChangesAsync();
        return subgroup;
    }

    public async Task<Subgroup?> UpdateAsync(int id, Subgroup subgroup)
    {
        var existingSubgroup = await _context.Subgroups.FindAsync(id);
        if (existingSubgroup == null)
            return null;

        existingSubgroup.Name = subgroup.Name;
        existingSubgroup.Description = subgroup.Description;
        existingSubgroup.GroupId = subgroup.GroupId;
        existingSubgroup.IsActive = subgroup.IsActive;

        await _context.SaveChangesAsync();
        return existingSubgroup;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subgroup = await _context.Subgroups.FindAsync(id);
        if (subgroup == null)
            return false;

        subgroup.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
