using AutoMapper;
using GoodHamburger.API.Data;
using GoodHamburger.API.DTOs;
using GoodHamburger.API.Models;
using GoodHamburger.API.Repositories.Interfaces;
using GoodHamburger.API.Services;
using GoodHamburger.Tests.Mocks.PedidosMock;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace GoodHamburger.Tests;

public class ValidadorPedidoTests
{
    private readonly Mock<IPedidoRepository> _repositoryMock;
    private readonly AppDbContext _context;
    private readonly PedidoService _service;

    public ValidadorPedidoTests()
    {
        _repositoryMock = new Mock<IPedidoRepository>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        _context.Itens.AddRange(
            PedidoMock.XBurger(),
            PedidoMock.XEgg(),
            PedidoMock.XBacon(),
            PedidoMock.BatataFrita(),
            PedidoMock.Refrigerante()
        );
        _context.SaveChanges();

        var configExpression = new MapperConfigurationExpression();
        configExpression.AddProfile<API.Mappers.PedidoProfile>();

        var mapperConfig = new MapperConfiguration(
            configExpression,
            LoggerFactory.Create(b => b.AddConsole())
        );

        _service = new PedidoService(_repositoryMock.Object, _context, mapperConfig.CreateMapper());
    }

    #region CriarAsync

    [Fact]
    public async Task CriarAsync_PedidoSemItens_DeveLancarExcecao()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int>() };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("O pedido deve conter ao menos um item.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_ItensDuplicados_DeveLancarExcecao()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 1 } };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("O pedido contém itens duplicados.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_IdInexistente_DeveLancarExcecao()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 99 } };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("Um ou mais itens informados não existem no cardápio.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_MaisDeUmSanduiche_DeveLancarExcecao()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 2 } };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("O pedido pode conter apenas um sanduíche.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_MaisDeUmaBatata_DeveLancarExcecao()
    {
        _context.Itens.Add(new Item { Id = 99, Nome = "Batata Extra", Preco = 2.00m, Tipo = TipoItem.Batata });
        _context.SaveChanges();

        var dto = new PedidoRequestDto { ItensId = new List<int> { 4, 99 } };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("O pedido pode conter apenas uma batata.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_MaisDeUmRefrigerante_DeveLancarExcecao()
    {
        _context.Itens.Add(new Item { Id = 98, Nome = "Suco", Preco = 2.50m, Tipo = TipoItem.Refrigerante });
        _context.SaveChanges();

        var dto = new PedidoRequestDto { ItensId = new List<int> { 5, 98 } };

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.CriarAsync(dto));

        Assert.Equal("O pedido pode conter apenas um refrigerante.", ex.Message);
    }

    [Fact]
    public async Task CriarAsync_PedidoValido_DeveRetornarPedidoCriado()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 4, 5 } };

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var resultado = await _service.CriarAsync(dto);

        Assert.NotNull(resultado);
        Assert.Equal(3, resultado.Itens.Count);
        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Pedido>()), Times.Once);
    }

    #endregion

    #region ListarTodosAsync

    [Fact]
    public async Task ListarTodosAsync_DeveRetornarTodosOsPedidos()
    {
        var pedidos = new List<Pedido>
        {
            new() { Id = 1, Itens = new List<Item> { PedidoMock.XBurger() }, Subtotal = 5.00m, Desconto = 0m, Total = 5.00m },
            new() { Id = 2, Itens = new List<Item> { PedidoMock.BatataFrita() }, Subtotal = 2.00m, Desconto = 0m, Total = 2.00m }
        };

        _repositoryMock
            .Setup(r => r.ListarTodosAsync())
            .ReturnsAsync(pedidos);

        var resultado = await _service.ListarTodosAsync();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count());
        _repositoryMock.Verify(r => r.ListarTodosAsync(), Times.Once);
    }

    [Fact]
    public async Task ListarTodosAsync_SemPedidos_DeveRetornarListaVazia()
    {
        _repositoryMock
            .Setup(r => r.ListarTodosAsync())
            .ReturnsAsync(new List<Pedido>());

        var resultado = await _service.ListarTodosAsync();

        Assert.NotNull(resultado);
        Assert.Empty(resultado);
    }

    #endregion

    #region ObterPorIdAsync

    [Fact]
    public async Task ObterPorIdAsync_PedidoExistente_DeveRetornarPedido()
    {
        var pedido = new Pedido { Id = 1, Itens = new List<Item> { PedidoMock.XBurger() }, Subtotal = 5.00m, Desconto = 0m, Total = 5.00m };

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync(pedido);

        var resultado = await _service.ObterPorIdAsync(1);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.Id);
        _repositoryMock.Verify(r => r.ObterPorIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_PedidoInexistente_DeveRetornarNulo()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(99))
            .ReturnsAsync((Pedido?)null);

        var resultado = await _service.ObterPorIdAsync(99);

        Assert.Null(resultado);
    }

    #endregion

    #region AtualizarAsync

    [Fact]
    public async Task AtualizarAsync_PedidoInexistente_DeveLancarExcecao()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(99))
            .ReturnsAsync((Pedido?)null);

        var dto = new PedidoRequestDto { ItensId = new List<int> { 1 } };

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.AtualizarAsync(99, dto));

        Assert.Equal("Pedido não encontrado.", ex.Message);
    }

    [Fact]
    public async Task AtualizarAsync_PedidoValido_DeveRetornarPedidoAtualizado()
    {
        var pedido = new Pedido { Id = 1, Itens = new List<Item> { PedidoMock.XBurger() } };

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync(pedido);

        _repositoryMock
            .Setup(r => r.AtualizarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 4 } };

        var resultado = await _service.AtualizarAsync(1, dto);

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Itens.Count);
        _repositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Once);
    }

    #endregion

    #region RemoverAsync

    [Fact]
    public async Task RemoverAsync_PedidoInexistente_DeveLancarExcecao()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(99))
            .ReturnsAsync((Pedido?)null);

        var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.RemoverAsync(99));

        Assert.Equal("Pedido não encontrado.", ex.Message);
    }

    [Fact]
    public async Task RemoverAsync_PedidoExistente_DeveRemoverComSucesso()
    {
        var pedido = new Pedido { Id = 1, Itens = new List<Item> { PedidoMock.XBurger() } };

        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(1))
            .ReturnsAsync(pedido);

        _repositoryMock
            .Setup(r => r.RemoverAsync(It.IsAny<Pedido>()))
            .Returns(Task.CompletedTask);

        await _service.RemoverAsync(1);

        _repositoryMock.Verify(r => r.RemoverAsync(It.IsAny<Pedido>()), Times.Once);
    }

    #endregion

    #region CalculadoraPedido

    [Fact]
    public async Task CriarAsync_SanduicheComBatataERefrigerante_DeveAplicar20PorCentoDesconto()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 4, 5 } };

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(9.50m, resultado.Subtotal);
        Assert.Equal(1.90m, resultado.Desconto);
        Assert.Equal(7.60m, resultado.Total);
    }

    [Fact]
    public async Task CriarAsync_SanduicheComRefrigerante_DeveAplicar15PorCentoDesconto()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 5 } };

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(7.50m, resultado.Subtotal);
        Assert.Equal(1.125m, resultado.Desconto);
        Assert.Equal(6.375m, resultado.Total);
    }

    [Fact]
    public async Task CriarAsync_SanduicheComBatata_DeveAplicar10PorCentoDesconto()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1, 4 } };

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(7.00m, resultado.Subtotal);
        Assert.Equal(0.70m, resultado.Desconto);
        Assert.Equal(6.30m, resultado.Total);
    }

    [Fact]
    public async Task CriarAsync_SemCombinacaoDeDesconto_NaoDeveAplicarDesconto()
    {
        var dto = new PedidoRequestDto { ItensId = new List<int> { 1 } };

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Pedido>()))
            .ReturnsAsync((Pedido p) => p);

        var resultado = await _service.CriarAsync(dto);

        Assert.Equal(5.00m, resultado.Subtotal);
        Assert.Equal(0m, resultado.Desconto);
        Assert.Equal(5.00m, resultado.Total);
    }

    #endregion
}