namespace PDVCasaVerde.DTOs;

public class GroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<SubgroupDto>? Subgroups { get; set; }
}

public class CreateGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateGroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class SubgroupDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
}

public class CreateSubgroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int GroupId { get; set; }
}

public class UpdateSubgroupDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int GroupId { get; set; }
    public bool IsActive { get; set; }
}
