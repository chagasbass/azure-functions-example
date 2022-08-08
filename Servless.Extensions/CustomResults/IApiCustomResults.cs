using Microsoft.Azure.Functions.Worker.Http;
using Serverless.Extensions.Entities;

namespace Serverless.Extensions.CustomResults;

public interface IApiCustomResults
{
    void GenerateLogResponse(CommandResult commandResult, int statusCode, bool hasError = true);
    Task<HttpResponseData> FormatApiResponse(CommandResult commandResult, HttpRequestData requestData, string? defaultEndpoint = null, string? resourceUri = null);
}