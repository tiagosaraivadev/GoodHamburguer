namespace GoodHamburger.Blazor.Models;

public class ErroRespostaModel
{
    public string? Mensagem { get; set; }
    public List<string>? Mensagens { get; set; }

    public string ObterMensagem()
    {
        if (Mensagens != null && Mensagens.Any())
            return string.Join(", ", Mensagens);
        return Mensagem ?? "Erro desconhecido.";
    }
}