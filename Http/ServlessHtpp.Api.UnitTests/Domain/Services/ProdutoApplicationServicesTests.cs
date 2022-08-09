using Serverless.Extensions.Entities;
using Serverless.Extensions.Shared.Enums;
using ServerlessHtpp.Api.Core.Application.Services;

namespace ServerlessHtpp.Api.UnitTests.Domain.Services;

public class ProdutoApplicationServicesTests
{
    private readonly Fixture _fixture;
    private readonly ProdutoDtoFake _produtoDtoFake;

    private readonly Mock<INotificationServices> _notificationServices;
    private readonly Mock<IProdutoRepository> _produtoRepository;
    private readonly Mock<IProdutoMessageServices> _produtoMessageServices;

    public ProdutoApplicationServicesTests()
    {
        _fixture = new Fixture();
        _produtoDtoFake = new ProdutoDtoFake(_fixture);
        _notificationServices = new Mock<INotificationServices>();
        _produtoRepository = new Mock<IProdutoRepository>();
        _produtoMessageServices = new Mock<IProdutoMessageServices>();
    }

    [Fact]
    [Trait("ProdutoApplicationServices", "Teste de validação de contrato")]
    public async Task Deve_Retornar_CommandResult_Contendo_Notificação_Quando_Produto_For_Inválido()
    {
        //Arrange
        var produtoDto = _produtoDtoFake.GerarEntidadeInvalida();

        var statusCode = StatusCodeOperation.BusinessError;

        _notificationServices.Setup(x => x.StatusCode).Returns(statusCode);

        //Act
        var produtoService = new ProdutoApplicationServices(_notificationServices.Object,
            _produtoRepository.Object, _produtoMessageServices.Object);

        var commandResult = (CommandResult)await produtoService.IntegrarProdutoAsync(produtoDto);

        //Assert
        Assert.False(commandResult.Success);
        Assert.NotNull(commandResult.Data);
        Assert.Equal(statusCode, _notificationServices.Object.StatusCode);
    }

    [Fact]
    [Trait("ProdutoApplicationServices", "Teste de inserção de novo produto")]
    public async Task Deve_Inserir_Produto_Quando_For_Válido_E_Não_Estiver_Cadastrado_No_Banco()
    {
        //Arrange
        var produtoDto = _produtoDtoFake.GerarEntidadeValida();
        Produto produto = produtoDto;

        Produto produtoDtoInvalido = null;

        _produtoRepository.Setup(x => x.ListarProdutoAsync(produtoDto.Nome)).ReturnsAsync(produtoDtoInvalido);
        _produtoRepository.Setup(x => x.InserirProdutoAsync(produto));

        //Act
        var produtoService = new ProdutoApplicationServices(_notificationServices.Object,
            _produtoRepository.Object, _produtoMessageServices.Object);

        var commandResult = (CommandResult)await produtoService.IntegrarProdutoAsync(produtoDto);

        //Assert
        Assert.True(commandResult.Success);
        Assert.NotNull(commandResult.Data);
        Assert.Equal(commandResult.Message, "Produto integrado com sucesso");
    }

    [Fact]
    [Trait("ProdutoApplicationServices", "Teste de atualização de produto")]
    public async Task Deve_Atualizar_Produto_Quando_For_Válido_E_Estiver_Cadastrado_No_Banco()
    {
        //Arrange
        var produtoDto = _produtoDtoFake.GerarEntidadeValida();
        Produto produto = produtoDto;

        _produtoRepository.Setup(x => x.ListarProdutoAsync(produtoDto.Nome)).ReturnsAsync(produto);
        _produtoRepository.Setup(x => x.AtualizarProdutoAsync(produto));

        //Act
        var produtoService = new ProdutoApplicationServices(_notificationServices.Object,
            _produtoRepository.Object, _produtoMessageServices.Object);

        var commandResult = (CommandResult)await produtoService.IntegrarProdutoAsync(produtoDto);

        //Assert
        Assert.True(commandResult.Success);
        Assert.NotNull(commandResult.Data);
        Assert.Equal(commandResult.Message, "Produto integrado com sucesso");
    }

}
