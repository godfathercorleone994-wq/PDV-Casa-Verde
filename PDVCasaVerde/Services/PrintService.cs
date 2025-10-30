using System.Text;
using PDVCasaVerde.Models;

namespace PDVCasaVerde.Services;

public class PrintService
{
    public string GenerateReceipt(Command command)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("========================================");
        sb.AppendLine("           CASA VERDE PDV");
        sb.AppendLine("========================================");
        sb.AppendLine($"COMANDA: {command.CommandNumber}");
        sb.AppendLine($"CLIENTE: {command.ClientName}");
        sb.AppendLine($"DATA/HORA: {command.OpenedAt:dd/MM/yyyy HH:mm}");
        sb.AppendLine("========================================");
        sb.AppendLine();
        sb.AppendLine("ITEM                    QTD   PREÇO");
        sb.AppendLine("----------------------------------------");
        
        foreach (var item in command.Items)
        {
            var name = item.Product.Name.Length > 20 
                ? item.Product.Name.Substring(0, 20) 
                : item.Product.Name.PadRight(20);
            sb.AppendLine($"{name}   {item.Quantity,3}   R$ {item.TotalPrice,7:F2}");
        }
        
        sb.AppendLine("----------------------------------------");
        sb.AppendLine($"TOTAL:                    R$ {command.TotalAmount,7:F2}");
        sb.AppendLine("========================================");
        
        if (command.ClosedAt.HasValue)
        {
            sb.AppendLine($"FECHADA EM: {command.ClosedAt:dd/MM/yyyy HH:mm}");
        }
        
        sb.AppendLine();
        sb.AppendLine("       OBRIGADO PELA PREFERÊNCIA!");
        sb.AppendLine("========================================");
        
        return sb.ToString();
    }

    public void PrintToConsole(Command command)
    {
        Console.Clear();
        Console.WriteLine(GenerateReceipt(command));
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
