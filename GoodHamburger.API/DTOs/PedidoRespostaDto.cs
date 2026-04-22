namespace GoodHamburger.API.DTOs
{
    public class PedidoRespostaDto
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public List<ItemRespostaDto> Itens { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
    }
}
