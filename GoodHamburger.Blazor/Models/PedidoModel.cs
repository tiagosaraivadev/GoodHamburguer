namespace GoodHamburger.Blazor.Models;

public class PedidoModel
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public List<ItemModel> Itens { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
}