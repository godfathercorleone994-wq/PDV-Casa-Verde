using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class GroupService
{
    private readonly PDVContext _context;

    public GroupService(PDVContext context)
    {
        _context = context;
    }

    public async Task<List<Group>> GetAllAsync()
    {
        return await _context.Groups
            .Include(g => g.Subgroups)
            .Where(g => g.IsActive)
            .ToListAsync();
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        return await _context.Groups
            .Include(g => g.Subgroups)
            .Include(g => g.Products)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Group> CreateAsync(Group group)
    {
        group.CreatedAt = DateTime.UtcNow;
        _context.Groups.Add(group);
        await _context.SaveChangesAsync();
        return group;
    }

    public async Task<Group?> UpdateAsync(int id, Group group)
    {
        var existingGroup = await _context.Groups.FindAsync(id);
        if (existingGroup == null)
            return null;

        existingGroup.Name = group.Name;
        existingGroup.Description = group.Description;
        existingGroup.IsActive = group.IsActive;

        await _context.SaveChangesAsync();
        return existingGroup;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var group = await _context.Groups.FindAsync(id);
        if (group == null)
            return false;

        group.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
