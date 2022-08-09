
namespace ServerlessHtpp.Api.UnitTests.Domain.Entities;

public class ProdutoTests
{
    private readonly Fixture _fixture;
    private readonly ProdutoDtoFake _produtoDtoFake;

    public ProdutoTests()
    {
        _fixture = new Fixture();
        _produtoDtoFake = new ProdutoDtoFake(_fixture);
    }


    [Fact]
    [Trait("Produto", "Testes de validação do produto")]
    public void Deve_Retornar_Notificacao_Quando_Produto_For_Invalido()
    {
        //Arrange
        Produto produto = _produtoDtoFake.GerarEntidadeInvalida();

        //Act
        produto.Validate();

        //Assert
        Assert.False(produto.IsValid);
        Assert.True(produto.Notifications.Any());
    }

    [Fact]
    [Trait("Produto", "Testes de validação do produto")]
    public void Não_Deve_Retornar_Notificacao_Quando_Produto_For_Válido()
    {
        //Arrange
        Produto produto = _produtoDtoFake.GerarEntidadeValida();

        //Act
        produto.Validate();

        //Assert
        Assert.True(produto.IsValid);
        Assert.False(produto.Notifications.Any());
    }
}
