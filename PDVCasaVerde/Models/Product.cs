namespace PDVCasaVerde.Models;

public class Product
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Foreign keys
    public int? GroupId { get; set; }
    public int? SubgroupId { get; set; }
    
    // Navigation properties
    public Group? Group { get; set; }
    public Subgroup? Subgroup { get; set; }
}
