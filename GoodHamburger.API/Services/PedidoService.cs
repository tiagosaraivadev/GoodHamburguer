using AutoMapper;
using GoodHamburger.API.Data;
using GoodHamburger.API.DTOs;
using GoodHamburger.API.Models;
using GoodHamburger.API.Repositories.Interfaces;
using GoodHamburger.API.Services.Interfaces;
using GoodHamburger.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.API.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PedidoService(IPedidoRepository repository,  AppDbContext context, IMapper mapper)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PedidoRespostaDto>> ListarTodosAsync()
        {
            var pedidos = await _repository.ListarTodosAsync();

            return _mapper.Map<IEnumerable<PedidoRespostaDto>>(pedidos);
        }

        public async Task<PedidoRespostaDto> ObterPorIdAsync(int id)
        {
            var pedido = await _repository.ObterPorIdAsync(id);

            return pedido is null ? null : _mapper.Map<PedidoRespostaDto>(pedido);
        }

        public async Task<PedidoRespostaDto> CriarAsync(PedidoRequestDto dto)
        {
            var itens = await _context.Itens
                .Where(i => dto.ItenIds.Contains(i.Id))
                .ToListAsync();

            PedidosUtils.ValidarItens(itens, dto.ItenIds);

            var pedido = new Pedido { Itens = itens };
            PedidosUtils.CalcularValores(pedido);

            await _repository.CriarAsync(pedido);

            return _mapper.Map<PedidoRespostaDto>(pedido);
        }

        public async Task<PedidoRespostaDto> AtualizarAsync(int id, PedidoRequestDto dto)
        {
            var pedido = await _repository.ObterPorIdAsync(id)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            var itens = await _context.Itens
                .Where(i => dto.ItenIds.Contains(i.Id))
                .ToListAsync();

            PedidosUtils.ValidarItens(itens, dto.ItenIds);

            pedido.Itens = itens;

            PedidosUtils.CalcularValores(pedido);

            await _repository.AtualizarAsync(pedido);

            return _mapper.Map<PedidoRespostaDto>(pedido);
        }

        public async Task RemoverAsync(int id)
        {
            var pedido = await _repository.ObterPorIdAsync(id)
                ?? throw new KeyNotFoundException("Pedido não encontrado.");

            await _repository.RemoverAsync(pedido);
        }
    }
}
