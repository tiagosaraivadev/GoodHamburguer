using GoodHamburger.API.Models;

namespace GoodHamburger.Tests.Mocks.PedidosMock
{
    public class PedidoMock
    {
        #region Cardápio
        public static Item XBurger() => new() { Id = 1, Nome = "X Burger", Preco = 5.00m, Tipo = TipoItem.Sanduiche };
        public static Item XEgg() => new() { Id = 2, Nome = "X Egg", Preco = 4.50m, Tipo = TipoItem.Sanduiche };
        public static Item XBacon() => new() { Id = 3, Nome = "X Bacon", Preco = 7.00m, Tipo = TipoItem.Sanduiche };
        public static Item BatataFrita() => new() { Id = 4, Nome = "Batata Frita", Preco = 2.00m, Tipo = TipoItem.Batata };
        public static Item Refrigerante() => new() { Id = 5, Nome = "Refrigerante", Preco = 2.50m, Tipo = TipoItem.Refrigerante };
        #endregion
    }
}
