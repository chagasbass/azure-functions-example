using ServerlessHtpp.Api.Core.Domain.Entities;

namespace ServerlessHtpp.Api.Core.Infra.Messaging.Interfaces;

public interface IProdutoMessageServices
{
    Task GerarEventoDeProdutoAtualizadoAsync(Produto produto);
}
