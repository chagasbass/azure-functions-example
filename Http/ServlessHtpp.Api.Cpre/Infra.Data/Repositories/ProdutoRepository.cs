using MongoDB.Driver;
using ServerlessHtpp.Api.Core.Domain.Entities;
using ServerlessHtpp.Api.Core.Domain.Repositories;
using ServerlessHtpp.Api.Core.Infra.Data.Contexts;

namespace ServerlessHtpp.Api.Core.Infra.Data.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly ProdutoDataContext _context;

    public ProdutoRepository(ProdutoDataContext context)
    {
        _context = context;
    }

    public async Task AtualizarProdutoAsync(Produto produto)
    {
        await _context.Produtos.ReplaceOneAsync(p => p.Id == produto.Id, produto);
    }

    public async Task InserirProdutoAsync(Produto produto)
    {
        await _context.Produtos.InsertOneAsync(produto);
    }

    public async Task<Produto> ListarProdutoAsync(string nome)
    {
        var produtos = await _context.Produtos
              .FindAsync(x => x.Nome.Equals(nome));

        return produtos.FirstOrDefault();
    }
}
