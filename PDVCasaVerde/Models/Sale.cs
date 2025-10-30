namespace PDVCasaVerde.Models;

public class Sale
{
    public int Id { get; set; }
    public int SaleNumber { get; set; }
    
    // Table or customer reference
    public int? TableNumber { get; set; }
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }
    public bool IsOpen { get; set; } = true;
    public decimal TotalAmount { get; set; }
    
    public string PaymentType { get; set; } = string.Empty; // "CASH", "CARD", "LEDGER", etc.
    public string Status { get; set; } = "OPEN"; // "OPEN", "CLOSED", "CANCELLED"
    
    // Navigation properties
    public List<SaleItem> Items { get; set; } = new();
}
