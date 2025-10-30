namespace PDVCasaVerde.Models;

public class Command
{
    public int Id { get; set; }
    public int CommandNumber { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public bool IsOpen { get; set; } = true;
    public decimal TotalAmount { get; set; }
    public List<CommandItem> Items { get; set; } = new();
}
