using GoodHamburger.API.Data;
using GoodHamburger.API.Models;
using GoodHamburger.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.API.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> ListarTodosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .ToListAsync();
    }

    public async Task<Pedido> ObterPorIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Pedido> CriarAsync(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<Pedido> AtualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task RemoverAsync(Pedido pedido)
    {
        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }
}