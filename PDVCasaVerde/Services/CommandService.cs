using Microsoft.EntityFrameworkCore;
using PDVCasaVerde.Data;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class CommandService
{
    private readonly PDVContext _context;

    public CommandService(PDVContext context)
    {
        _context = context;
    }

    public async Task<Command> CreateCommandAsync(string clientName)
    {
        var lastCommand = await _context.Commands
            .OrderByDescending(c => c.CommandNumber)
            .FirstOrDefaultAsync();

        var command = new Command
        {
            CommandNumber = (lastCommand?.CommandNumber ?? 0) + 1,
            ClientName = clientName,
            OpenedAt = DateTime.Now,
            IsOpen = true,
            TotalAmount = 0
        };

        _context.Commands.Add(command);
        await _context.SaveChangesAsync();
        return command;
    }

    public async Task<Command?> GetCommandByNumberAsync(int commandNumber)
    {
        return await _context.Commands
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CommandNumber == commandNumber);
    }

    public async Task<List<Command>> GetOpenCommandsAsync()
    {
        return await _context.Commands
            .Where(c => c.IsOpen)
            .OrderBy(c => c.CommandNumber)
            .ToListAsync();
    }

    public async Task<bool> AddItemToCommandAsync(int commandNumber, int productCode, int quantity)
    {
        var command = await GetCommandByNumberAsync(commandNumber);
        if (command == null || !command.IsOpen) return false;

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Code == productCode && p.IsActive);
        if (product == null) return false;

        var item = new CommandItem
        {
            CommandId = command.Id,
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price,
            TotalPrice = product.Price * quantity,
            AddedAt = DateTime.Now
        };

        _context.CommandItems.Add(item);
        
        command.TotalAmount += item.TotalPrice;
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<Command?> CloseCommandAsync(int commandNumber)
    {
        var command = await GetCommandByNumberAsync(commandNumber);
        if (command == null || !command.IsOpen) return null;

        command.IsOpen = false;
        command.ClosedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        
        return command;
    }
}
