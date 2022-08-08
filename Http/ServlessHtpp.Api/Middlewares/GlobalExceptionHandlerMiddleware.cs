using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Options;
using Serverless.Extensions.Entities;
using Serverless.Extensions.Factories;
using Serverless.Extensions.Shared.Configurations;
using Serverless.Extensions.Shared.Enums;
using Serverless.Extensions.Shared.Logs.Services;
using System.Text.Json;

namespace ServerlessHttp.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ProblemDetailConfigurationOptions _problemOptions;
    private readonly ILogServices _logServices;

    public GlobalExceptionHandlerMiddleware(IOptions<ProblemDetailConfigurationOptions> problemOptions,
                                            ILogServices logServices)
    {
        _problemOptions = problemOptions.Value;
        _logServices = logServices;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Responsavel por tratar as exceções geradas na aplicação
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public async Task HandleExceptionAsync(FunctionContext context, Exception exception)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;
        const string dataType = @"application/problem+json";

        _logServices.LogData.AddException(exception);
        _logServices.LogData.AddResponseStatusCode(statusCode);
        _logServices.WriteLog();
        _logServices.WriteLogWhenRaiseExceptions();

        var problemDetails = await ConfigurarDetalhesDoProblema(statusCode, exception, context);

        var commandResult = new CommandResult(problemDetails);

        var request = await context.GetHttpRequestDataAsync();

        var response = request!.CreateResponse();
        response.Headers.Add("Content-Type", dataType);
        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;

        await response.WriteStringAsync(JsonSerializer.Serialize(commandResult, JsonOptionsFactory.GetSerializerOptions()));

        context.GetInvocationResult().Value = response;
    }

    private async Task<ProblemDetails> ConfigurarDetalhesDoProblema(int statusCode, Exception exception, FunctionContext context)
    {
        var defaultTitle = "Um erro ocorreu ao processar o request.";
        var defaultDetail = $"Erro fatal na aplicação,entre em contato com um Desenvolvedor responsável. Causa: {exception.Message}";

        var request = await context.GetHttpRequestDataAsync();

        var title = _problemOptions.Title;
        var detail = _problemOptions.Detail;
        var instance = request.Url.ToString();

        var type = StatusCodeOperation.RetrieveStatusCode(statusCode);

        if (string.IsNullOrEmpty(title))
            title = defaultTitle;

        if (string.IsNullOrEmpty(detail))
            detail = defaultDetail;

        return new ProblemDetails()
        {
            Detail = detail,
            Instance = instance,
            Status = statusCode,
            Title = title,
            Type = type.Text
        };
    }
}
