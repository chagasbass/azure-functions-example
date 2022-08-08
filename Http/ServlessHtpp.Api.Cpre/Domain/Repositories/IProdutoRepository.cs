using ServerlessHtpp.Api.Core.Domain.Entities;

namespace ServerlessHtpp.Api.Core.Domain.Repositories;

public interface IProdutoRepository
{
    Task<Produto> ListarProdutoAsync(string nome);
    Task InserirProdutoAsync(Produto produto);
    Task AtualizarProdutoAsync(Produto produto);
}
