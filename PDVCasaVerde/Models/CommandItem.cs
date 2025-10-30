namespace PDVCasaVerde.Models;

public class CommandItem
{
    public int Id { get; set; }
    public int CommandId { get; set; }
    public Command Command { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime AddedAt { get; set; }
}
