using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Serverless.Extensions.CustomResults;
using Serverless.Extensions.Entities;
using ServerlessHtpp.Api.Core.Application.Dtos;
using ServerlessHtpp.Api.Core.Application.Interfaces;
using System.Net;
using System.Text.Json;

namespace ServerlessHtpp.Api.Functions;

public class ClienteHttpFunction
{
    private readonly IApiCustomResults _apiCustomResults;
    private readonly IProdutoApplicationServices _produtoApplicationServices;

    private readonly string defaultEndpoint = @"v1/produtos/";

    public ClienteHttpFunction(IApiCustomResults apiCustomResults, IProdutoApplicationServices produtoApplicationServices)
    {
        _apiCustomResults = apiCustomResults;
        _produtoApplicationServices = produtoApplicationServices;
    }

    [Function("IntegrarProdutosAsync")]
    [OpenApiOperation(operationId: "Integração de Produtos", tags: new[] { "Produtos" })]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(ProdutoDto), Example = typeof(ProdutoDto), Required = true)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.Created, contentType: "application/json", bodyType: typeof(ProdutoDto), Description = "Resultado na integração do produto")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(ICommandResult), Description = "Falha na integração do produto")]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/produtos")] HttpRequestData requestData)
    {
        var produtoJson = requestData.ReadAsString();

        var produto = JsonSerializer.Deserialize<ProdutoDto>(produtoJson);

        var commandResult = (CommandResult)await _produtoApplicationServices.IntegrarProdutoAsync(produto);

        return await _apiCustomResults.FormatApiResponse(commandResult, requestData);
    }
}
