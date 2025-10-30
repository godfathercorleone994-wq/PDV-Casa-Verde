namespace PDVCasaVerde.Models;

public class Commission
{
    public int Id { get; set; }
    public int CommandId { get; set; }
    public Command Command { get; set; } = null!;
    public string StaffName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Notes { get; set; }
}
