namespace PDVCasaVerde.DTOs;

public class SaleDto
{
    public int Id { get; set; }
    public int SaleNumber { get; set; }
    public int? TableNumber { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
    public bool IsOpen { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<SaleItemDto>? Items { get; set; }
}

public class CreateSaleDto
{
    public int? TableNumber { get; set; }
    public int? CustomerId { get; set; }
}

public class AddSaleItemDto
{
    public int ProductCode { get; set; }
    public int Quantity { get; set; }
}

public class CloseSaleDto
{
    public string PaymentType { get; set; } = "CASH"; // CASH, CARD, LEDGER, etc.
}

public class SaleItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime AddedAt { get; set; }
}
