namespace PDVCasaVerde.Models;

public class Subgroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public int GroupId { get; set; }
    
    // Navigation properties
    public Group Group { get; set; } = null!;
    public List<Product> Products { get; set; } = new();
}
