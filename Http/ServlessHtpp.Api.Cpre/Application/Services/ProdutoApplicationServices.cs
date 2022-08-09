using Serverless.Extensions.Entities;
using Serverless.Extensions.Shared.Enums;
using Serverless.Extensions.Shared.Notifications;
using ServerlessHtpp.Api.Core.Application.Dtos;
using ServerlessHtpp.Api.Core.Application.Interfaces;
using ServerlessHtpp.Api.Core.Domain.Entities;
using ServerlessHtpp.Api.Core.Domain.Repositories;
using ServerlessHtpp.Api.Core.Infra.Messaging.Interfaces;

namespace ServerlessHtpp.Api.Core.Application.Services;

public class ProdutoApplicationServices : IProdutoApplicationServices
{
    private readonly INotificationServices _notificationServices;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoMessageServices _produtoMessageServices;

    public ProdutoApplicationServices(INotificationServices notificationServices,
                                      IProdutoRepository produtoRepository,
                                      IProdutoMessageServices produtoMessageServices)
    {
        _notificationServices = notificationServices;
        _produtoRepository = produtoRepository;
        _produtoMessageServices = produtoMessageServices;
    }

    public async Task<ICommandResult> IntegrarProdutoAsync(ProdutoDto produtoDto)
    {
        Produto produto = produtoDto;

        produto.Validate();

        if (!produto.IsValid)
        {
            _notificationServices.AddNotifications(produto.Notifications.ToList(), StatusCodeOperation.BusinessError);

            return new CommandResult(produto.Notifications.ToList(), false, "Problemas na integração do Produto");
        }

        var produtoRecuperado = await _produtoRepository.ListarProdutoAsync(produto.Nome);

        if (produtoRecuperado is null)
        {
            produtoRecuperado = produto;
            await _produtoRepository.InserirProdutoAsync(produtoRecuperado);
        }
        else
        {
            produtoRecuperado.Nome = produto.Nome;
            produtoRecuperado.Quantidade = produto.Quantidade;
            produtoRecuperado.Preco = produto.Preco;

            await _produtoRepository.AtualizarProdutoAsync(produtoRecuperado);
        }

        await _produtoMessageServices.GerarEventoDeProdutoAtualizadoAsync(produtoRecuperado);

        _notificationServices.AddStatusCode(StatusCodeOperation.Created);

        return new CommandResult(produtoRecuperado, true, "Produto integrado com sucesso");
    }
}
