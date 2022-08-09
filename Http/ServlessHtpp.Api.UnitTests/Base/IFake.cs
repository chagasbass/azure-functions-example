namespace ServerlessHtpp.Api.UnitTests.Base
{
    public interface IFake<T>
    {
        T GerarEntidadeValida();
        T GerarEntidadeInvalida();
    }
}
