using Microsoft.AspNetCore.Mvc;
using PDVCasaVerde.DTOs;
using PDVCasaVerde.Services;

namespace PDVCasaVerde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly SaleService _saleService;

    public SalesController(SaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<SaleDto>>> GetOpenSales()
    {
        var sales = await _saleService.GetOpenSalesAsync();
        var dtos = sales.Select(s => MapToDto(s)).ToList();
        return Ok(dtos);
    }

    [HttpGet("table/{tableNumber}")]
    public async Task<ActionResult<List<SaleDto>>> GetByTable(int tableNumber)
    {
        var sales = await _saleService.GetSalesByTableAsync(tableNumber);
        var dtos = sales.Select(s => MapToDto(s)).ToList();
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetById(int id)
    {
        var sale = await _saleService.GetSaleAsync(id);
        if (sale == null)
            return NotFound(new { message = "Venda não encontrada" });

        return Ok(MapToDto(sale));
    }

    [HttpGet("number/{saleNumber}")]
    public async Task<ActionResult<SaleDto>> GetByNumber(int saleNumber)
    {
        var sale = await _saleService.GetSaleByNumberAsync(saleNumber);
        if (sale == null)
            return NotFound(new { message = "Venda não encontrada" });

        return Ok(MapToDto(sale));
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> Create([FromBody] CreateSaleDto dto)
    {
        var sale = await _saleService.CreateSaleAsync(dto.TableNumber, dto.CustomerId);
        return CreatedAtAction(nameof(GetById), new { id = sale.Id }, MapToDto(sale));
    }

    [HttpPost("{id}/items")]
    public async Task<ActionResult> AddItem(int id, [FromBody] AddSaleItemDto dto)
    {
        var success = await _saleService.AddItemToSaleAsync(id, dto.ProductCode, dto.Quantity);
        if (!success)
            return BadRequest(new { message = "Erro ao adicionar item. Verifique se a venda está aberta e o produto existe." });

        var sale = await _saleService.GetSaleAsync(id);
        return Ok(MapToDto(sale!));
    }

    [HttpPost("{id}/close")]
    public async Task<ActionResult<SaleDto>> Close(int id, [FromBody] CloseSaleDto dto)
    {
        var sale = await _saleService.CloseSaleAsync(id, dto.PaymentType);
        if (sale == null)
            return NotFound(new { message = "Venda não encontrada ou já está fechada" });

        return Ok(MapToDto(sale));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ActionResult> Cancel(int id)
    {
        var success = await _saleService.CancelSaleAsync(id);
        if (!success)
            return NotFound(new { message = "Venda não encontrada ou já está fechada" });

        return NoContent();
    }

    private static SaleDto MapToDto(Models.Sale sale)
    {
        return new SaleDto
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            TableNumber = sale.TableNumber,
            CustomerId = sale.CustomerId,
            CustomerName = sale.Customer?.Name,
            OpenedAt = sale.OpenedAt,
            ClosedAt = sale.ClosedAt,
            IsOpen = sale.IsOpen,
            TotalAmount = sale.TotalAmount,
            PaymentType = sale.PaymentType,
            Status = sale.Status,
            Items = sale.Items?.Select(i => new SaleItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice,
                AddedAt = i.AddedAt
            }).ToList()
        };
    }
}
