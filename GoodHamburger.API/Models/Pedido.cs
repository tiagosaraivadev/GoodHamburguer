namespace GoodHamburger.API.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public List<Item> Itens { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
    }
}
