using AutoFixture;
using ServerlessHtpp.Api.Core.Domain.Entities;
using ServerlessHtpp.Api.UnitTests.Base;

namespace ServerlessHtpp.Api.UnitTests.Domain.Fakes
{
    public class ProdutoFake : IFake<Produto>
    {
        private readonly Fixture _fixture;

        public ProdutoFake(Fixture fixture)
        {
            _fixture = fixture;
        }

        public Produto GerarEntidadeInvalida()
        {
            return null;
        }

        public Produto GerarEntidadeValida()
        {
            return _fixture.Build<Produto>().Create();
        }
    }
}
