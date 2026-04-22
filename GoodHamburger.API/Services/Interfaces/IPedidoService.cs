using GoodHamburger.API.DTOs;

namespace GoodHamburger.API.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<PedidoRespostaDto>> ListarTodosAsync();
        Task<PedidoRespostaDto> ObterPorIdAsync(int Id);
        Task<PedidoRespostaDto> CriarAsync(PedidoRequestDto dto);
        Task<PedidoRespostaDto> AtualizarAsync(int id, PedidoRequestDto dto);
        Task RemoverAsync(int id);
    }
}
