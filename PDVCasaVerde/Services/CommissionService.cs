using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class CommissionService
{
    private readonly PDVContext _context;

    public CommissionService(PDVContext context)
    {
        _context = context;
    }

    public async Task<Commission?> AddCommissionAsync(int commandNumber, string staffName, decimal amount, string? notes = null)
    {
        var command = await _context.Commands
            .FirstOrDefaultAsync(c => c.CommandNumber == commandNumber);
        
        if (command == null) return null;

        var commission = new Commission
        {
            CommandId = command.Id,
            StaffName = staffName,
            Amount = amount,
            CreatedAt = DateTime.Now,
            Notes = notes
        };

        _context.Commissions.Add(commission);
        await _context.SaveChangesAsync();
        
        return commission;
    }

    public async Task<List<Commission>> GetCommissionsByStaffAsync(string staffName)
    {
        return await _context.Commissions
            .Include(c => c.Command)
            .Where(c => c.StaffName == staffName)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Commission>> GetCommissionsByCommandAsync(int commandNumber)
    {
        return await _context.Commissions
            .Include(c => c.Command)
            .Where(c => c.Command.CommandNumber == commandNumber)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalCommissionsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Commissions.AsQueryable();

        if (startDate.HasValue)
            query = query.Where(c => c.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(c => c.CreatedAt <= endDate.Value);

        return await query.SumAsync(c => c.Amount);
    }
}
