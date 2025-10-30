using Microsoft.AspNetCore.Mvc;
using PDVCasaVerde.DTOs;
using PDVCasaVerde.Models;
using PDVCasaVerde.Services;

namespace PDVCasaVerde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubgroupsController : ControllerBase
{
    private readonly SubgroupService _subgroupService;

    public SubgroupsController(SubgroupService subgroupService)
    {
        _subgroupService = subgroupService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SubgroupDto>>> GetAll()
    {
        var subgroups = await _subgroupService.GetAllAsync();
        var dtos = subgroups.Select(s => new SubgroupDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            IsActive = s.IsActive,
            GroupId = s.GroupId,
            GroupName = s.Group?.Name
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("group/{groupId}")]
    public async Task<ActionResult<List<SubgroupDto>>> GetByGroupId(int groupId)
    {
        var subgroups = await _subgroupService.GetByGroupIdAsync(groupId);
        var dtos = subgroups.Select(s => new SubgroupDto
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            IsActive = s.IsActive,
            GroupId = s.GroupId,
            GroupName = s.Group?.Name
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubgroupDto>> GetById(int id)
    {
        var subgroup = await _subgroupService.GetByIdAsync(id);
        if (subgroup == null)
            return NotFound(new { message = "Subgrupo não encontrado" });

        var dto = new SubgroupDto
        {
            Id = subgroup.Id,
            Name = subgroup.Name,
            Description = subgroup.Description,
            IsActive = subgroup.IsActive,
            GroupId = subgroup.GroupId,
            GroupName = subgroup.Group?.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<SubgroupDto>> Create([FromBody] CreateSubgroupDto dto)
    {
        var subgroup = new Subgroup
        {
            Name = dto.Name,
            Description = dto.Description,
            GroupId = dto.GroupId
        };

        var created = await _subgroupService.CreateAsync(subgroup);
        var resultDto = new SubgroupDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            IsActive = created.IsActive,
            GroupId = created.GroupId
        };

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubgroupDto>> Update(int id, [FromBody] UpdateSubgroupDto dto)
    {
        var subgroup = new Subgroup
        {
            Name = dto.Name,
            Description = dto.Description,
            GroupId = dto.GroupId,
            IsActive = dto.IsActive
        };

        var updated = await _subgroupService.UpdateAsync(id, subgroup);
        if (updated == null)
            return NotFound(new { message = "Subgrupo não encontrado" });

        var resultDto = new SubgroupDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            IsActive = updated.IsActive,
            GroupId = updated.GroupId
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _subgroupService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Subgrupo não encontrado" });

        return NoContent();
    }
}
