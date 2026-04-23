using GoodHamburger.API.Models;

namespace GoodHamburger.API.Utils
{
    public class PedidosUtils
    {
        private static readonly List<Action<List<Item>, List<int>>> _regras = new()
        {
            (itens, ids) =>
            {
                if (ids == null || ids.Count == 0)
                    throw new ArgumentException("O pedido deve conter ao menos um item.");
            },
            (itens, ids) =>
            {
                if (ids.Count != ids.Distinct().Count())
                    throw new ArgumentException("O pedido contém itens duplicados.");
            },
            (itens, ids) =>
            {
                if (itens.Count != ids.Count)
                    throw new ArgumentException("Um ou mais itens informados não existem no cardápio.");
            },
            (itens, ids) =>
            {
                if (itens.Count(i => i.Tipo == TipoItem.Sanduiche) > 1)
                    throw new ArgumentException("O pedido pode conter apenas um sanduíche.");
            },
            (itens, ids) =>
            {
                if (itens.Count(i => i.Tipo == TipoItem.Batata) > 1)
                    throw new ArgumentException("O pedido pode conter apenas uma batata.");
            },
            (itens, ids) =>
            {
                if (itens.Count(i => i.Tipo == TipoItem.Refrigerante) > 1)
                    throw new ArgumentException("O pedido pode conter apenas um refrigerante.");
            }
        };

        public static void CalcularValores(Pedido pedido)
        {
            pedido.Subtotal = pedido.Itens.Sum(i => i.Preco);

            bool temSanduiche = pedido.Itens.Any(i => i.Tipo == TipoItem.Sanduiche);
            bool temBatata = pedido.Itens.Any(i => i.Tipo == TipoItem.Batata);
            bool temRefrigerante = pedido.Itens.Any(i => i.Tipo == TipoItem.Refrigerante);

            pedido.Desconto = (temSanduiche, temBatata, temRefrigerante) switch
            {
                (true, true, true) => pedido.Subtotal * 0.20m,
                (true, false, true) => pedido.Subtotal * 0.15m,
                (true, true, false) => pedido.Subtotal * 0.10m,
                _ => 0m
            };

            pedido.Total = pedido.Subtotal - pedido.Desconto;
        }

        public static void ValidarItens(List<Item> itens, List<int> idsRecebidos)
        {
            foreach (var regra in _regras)
                regra(itens, idsRecebidos);
        }
    }
}
