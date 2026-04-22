using AutoMapper;
using GoodHamburger.API.Data;
using GoodHamburger.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardapioController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CardapioController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("listar-itens")]
    public async Task<IActionResult> ObterCardapio()
    {
        var itens = await _context.Itens.ToListAsync();
        var dto = _mapper.Map<IEnumerable<ItemRespostaDto>>(itens);
        return Ok(dto);
    }
}