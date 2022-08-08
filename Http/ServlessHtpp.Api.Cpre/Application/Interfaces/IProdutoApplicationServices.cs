using Serverless.Extensions.Entities;
using ServerlessHtpp.Api.Core.Application.Dtos;

namespace ServerlessHtpp.Api.Core.Application.Interfaces
{
    public interface IProdutoApplicationServices
    {
        Task<ICommandResult> IntegrarProdutoAsync(ProdutoDto produtoDto);
    }
}
