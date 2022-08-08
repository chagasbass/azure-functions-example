using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Serverless.Extensions.Shared.Configurations;
using ServerlessHtpp.Api.Core.Domain.Entities;

namespace ServerlessHtpp.Api.Core.Infra.Data.Contexts;

public class ProdutoDataContext : IProdutoDataContext
{
    //mongodb://admin:thi180798@ds035747.mlab.com:35747/despensa_db
    private readonly BaseConfigurationOptions _options;
    public IMongoDatabase ConexaoMongo { get; private set; }

    public ProdutoDataContext(IOptions<BaseConfigurationOptions> options)
    {
        _options = options.Value;
        Conectar();
    }

    public void Conectar() => ConfigurarConexoes();

    private void ConfigurarConexoes()
    {
        var configuracoes = MongoClientSettings.FromConnectionString(_options.StringConexaoBancoDeDados);
        configuracoes.MaxConnectionIdleTime = TimeSpan.FromSeconds(30);
        configuracoes.UseTls = false;
        configuracoes.RetryWrites = true;
        configuracoes.ServerApi = new ServerApi(ServerApiVersion.V1);

        var cliente = new MongoClient(configuracoes);

        ConexaoMongo = cliente.GetDatabase(_options.NomeBanco);
    }

    #region Coleções
    public IMongoCollection<Produto> Produtos => ConexaoMongo.GetCollection<Produto>("Produtos");

    #endregion
}
