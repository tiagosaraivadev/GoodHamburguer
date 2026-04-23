using GoodHamburger.Blazor.Models;
using System.Net.Http.Json;

namespace GoodHamburger.Blazor.Services;

public class PedidoService
{
    private readonly HttpClient _http;

    public PedidoService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ItemModel>> ObterCardapioAsync()
        => await _http.GetFromJsonAsync<List<ItemModel>>("api/cardapio/listar-itens") ?? new();

    public async Task<List<PedidoModel>> ListarPedidosAsync()
        => await _http.GetFromJsonAsync<List<PedidoModel>>("api/pedido/listar-todos") ?? new();

    public async Task<PedidoModel?> ObterPedidoPorIdAsync(int id)
        => await _http.GetFromJsonAsync<PedidoModel>($"api/pedido/buscar-por-id/{id}");

    public async Task CriarPedidoAsync(PedidoRequestModel request)
    {
        var resposta = await _http.PostAsJsonAsync("api/pedido/criar", request);
        if (!resposta.IsSuccessStatusCode)
        {
            var erro = await resposta.Content.ReadFromJsonAsync<ErroRespostaModel>();
            throw new Exception(erro?.ObterMensagem() ?? "Erro ao criar pedido.");
        }
    }

    public async Task AtualizarPedidoAsync(int id, PedidoRequestModel request)
    {
        var resposta = await _http.PutAsJsonAsync($"api/pedido/atualizar/{id}", request);
        if (!resposta.IsSuccessStatusCode)
        {
            var erro = await resposta.Content.ReadFromJsonAsync<ErroRespostaModel>();
            throw new Exception(erro?.ObterMensagem() ?? "Erro ao atualizar pedido.");
        }
    }

    public async Task RemoverPedidoAsync(int id)
        => await _http.DeleteAsync($"api/pedido/remover/{id}");
}