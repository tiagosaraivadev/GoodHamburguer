namespace GoodHamburger.Blazor.Models;

public class ItemModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Tipo { get; set; } = string.Empty;
}