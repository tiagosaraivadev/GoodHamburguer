using GoodHamburger.API.Models;

namespace GoodHamburger.API.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ListarTodosAsync();
        Task<Pedido> ObterPorIdAsync(int Id);
        Task<Pedido> CriarAsync(Pedido pedido);
        Task<Pedido> AtualizarAsync(Pedido pedido);
        Task RemoverAsync(Pedido pedido);
    }
}
