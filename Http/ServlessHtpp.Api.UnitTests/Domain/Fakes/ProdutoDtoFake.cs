using ServerlessHtpp.Api.Core.Application.Dtos;
using ServerlessHtpp.Api.UnitTests.Base;

namespace ServerlessHtpp.Api.UnitTests.Domain.Fakes
{
    public class ProdutoDtoFake : IFake<ProdutoDto>
    {
        private readonly Fixture _fixture;

        public ProdutoDtoFake(Fixture fixture)
        {
            _fixture = fixture;
        }

        public ProdutoDto GerarEntidadeInvalida()
        {
            return _fixture.Build<ProdutoDto>()
                           .Without(x => x.Nome)
                           .Do(x =>
                           {
                               x.Nome = "";
                           })
                           .Create();
        }

        public ProdutoDto GerarEntidadeValida()
        {
            return _fixture.Build<ProdutoDto>().Create();
        }
    }
}
