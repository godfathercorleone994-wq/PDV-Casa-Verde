namespace PDVCasaVerde.Models;

public class CustomerLedgerEntry
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    
    public string Type { get; set; } = string.Empty; // "SALE" or "PAYMENT"
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Optional: Link to sale if this is a sale entry
    public int? SaleId { get; set; }
}
