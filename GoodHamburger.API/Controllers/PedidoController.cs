using GoodHamburger.API.DTOs;
using GoodHamburger.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidoController(IPedidoService service)
    {
        _service = service;
    }

    [HttpGet("listar-todos")]
    public async Task<IActionResult> ListarTodos()
    {
        var pedidos = await _service.ListarTodosAsync();
        return Ok(pedidos);
    }

    [HttpGet("buscar-por-id/{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {
        var pedido = await _service.ObterPorIdAsync(id);

        if (pedido is null)
            return NotFound(new { mensagem = "Pedido não encontrado." });

        return Ok(pedido);
    }

    [HttpPost("criar")]
    public async Task<IActionResult> Criar([FromBody] PedidoRequestDto dto)
    {
        try
        {
            var pedido = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("atualizar/{id}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] PedidoRequestDto dto)
    {
        try
        {
            var pedido = await _service.AtualizarAsync(id, dto);
            return Ok(pedido);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpDelete("remover/{id}")]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { mensagem = ex.Message });
        }
    }
}