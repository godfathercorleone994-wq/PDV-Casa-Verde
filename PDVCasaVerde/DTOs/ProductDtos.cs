namespace PDVCasaVerde.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? GroupId { get; set; }
    public int? SubgroupId { get; set; }
    public string? GroupName { get; set; }
    public string? SubgroupName { get; set; }
}

public class CreateProductDto
{
    public int Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public int? GroupId { get; set; }
    public int? SubgroupId { get; set; }
}

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int? GroupId { get; set; }
    public int? SubgroupId { get; set; }
}
