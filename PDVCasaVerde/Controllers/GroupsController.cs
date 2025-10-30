using Microsoft.AspNetCore.Mvc;
using PDVCasaVerde.DTOs;
using PDVCasaVerde.Models;
using PDVCasaVerde.Services;

namespace PDVCasaVerde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupsController(GroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupDto>>> GetAll()
    {
        var groups = await _groupService.GetAllAsync();
        var dtos = groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description,
            IsActive = g.IsActive,
            Subgroups = g.Subgroups.Select(s => new SubgroupDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsActive = s.IsActive,
                GroupId = s.GroupId
            }).ToList()
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GroupDto>> GetById(int id)
    {
        var group = await _groupService.GetByIdAsync(id);
        if (group == null)
            return NotFound(new { message = "Grupo não encontrado" });

        var dto = new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            IsActive = group.IsActive,
            Subgroups = group.Subgroups.Select(s => new SubgroupDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                IsActive = s.IsActive,
                GroupId = s.GroupId
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<GroupDto>> Create([FromBody] CreateGroupDto dto)
    {
        var group = new Group
        {
            Name = dto.Name,
            Description = dto.Description
        };

        var created = await _groupService.CreateAsync(group);
        var resultDto = new GroupDto
        {
            Id = created.Id,
            Name = created.Name,
            Description = created.Description,
            IsActive = created.IsActive
        };

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GroupDto>> Update(int id, [FromBody] UpdateGroupDto dto)
    {
        var group = new Group
        {
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };

        var updated = await _groupService.UpdateAsync(id, group);
        if (updated == null)
            return NotFound(new { message = "Grupo não encontrado" });

        var resultDto = new GroupDto
        {
            Id = updated.Id,
            Name = updated.Name,
            Description = updated.Description,
            IsActive = updated.IsActive
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _groupService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Grupo não encontrado" });

        return NoContent();
    }
}
