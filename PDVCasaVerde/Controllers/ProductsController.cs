using Microsoft.AspNetCore.Mvc;
using PDVCasaVerde.DTOs;
using PDVCasaVerde.Models;
using PDVCasaVerde.Services;

namespace PDVCasaVerde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Code = p.Code,
            Name = p.Name,
            Price = p.Price,
            Category = p.Category,
            IsActive = p.IsActive,
            GroupId = p.GroupId,
            SubgroupId = p.SubgroupId,
            GroupName = p.Group?.Name,
            SubgroupName = p.Subgroup?.Name
        }).ToList();

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Produto não encontrado" });

        var dto = new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            IsActive = product.IsActive,
            GroupId = product.GroupId,
            SubgroupId = product.SubgroupId,
            GroupName = product.Group?.Name,
            SubgroupName = product.Subgroup?.Name
        };

        return Ok(dto);
    }

    [HttpGet("code/{code}")]
    public async Task<ActionResult<ProductDto>> GetByCode(int code)
    {
        var product = await _productService.GetByCodeAsync(code);
        if (product == null)
            return NotFound(new { message = "Produto não encontrado" });

        var dto = new ProductDto
        {
            Id = product.Id,
            Code = product.Code,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category,
            IsActive = product.IsActive,
            GroupId = product.GroupId,
            SubgroupId = product.SubgroupId,
            GroupName = product.Group?.Name,
            SubgroupName = product.Subgroup?.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product
        {
            Code = dto.Code,
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            GroupId = dto.GroupId,
            SubgroupId = dto.SubgroupId
        };

        try
        {
            var created = await _productService.CreateAsync(product);
            var resultDto = new ProductDto
            {
                Id = created.Id,
                Code = created.Code,
                Name = created.Name,
                Price = created.Price,
                Category = created.Category,
                IsActive = created.IsActive,
                GroupId = created.GroupId,
                SubgroupId = created.SubgroupId
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, resultDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Category = dto.Category,
            IsActive = dto.IsActive,
            GroupId = dto.GroupId,
            SubgroupId = dto.SubgroupId
        };

        var updated = await _productService.UpdateAsync(id, product);
        if (updated == null)
            return NotFound(new { message = "Produto não encontrado" });

        var resultDto = new ProductDto
        {
            Id = updated.Id,
            Code = updated.Code,
            Name = updated.Name,
            Price = updated.Price,
            Category = updated.Category,
            IsActive = updated.IsActive,
            GroupId = updated.GroupId,
            SubgroupId = updated.SubgroupId
        };

        return Ok(resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success)
            return NotFound(new { message = "Produto não encontrado" });

        return NoContent();
    }
}
