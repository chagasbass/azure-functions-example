using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Serverless.Extensions.Entities;
using Serverless.Extensions.Factories;
using Serverless.Extensions.Shared.Enums;
using Serverless.Extensions.Shared.Logs.Services;
using Serverless.Extensions.Shared.Notifications;
using System.Net;
using System.Text.Json;

namespace Serverless.Extensions.CustomResults;

public class ApiCustomResults : IApiCustomResults
{
    private readonly ILogServices _logServices;
    private readonly INotificationServices _notificationServices;

    public ApiCustomResults(ILogServices logServices,
                            INotificationServices notificationServices)
    {
        _logServices = logServices;
        _notificationServices = notificationServices;
    }

    public async Task<HttpResponseData> FormatApiResponse(CommandResult commandResult, HttpRequestData requestData, string? defaultEndpointRoute = null, string? resourceUri = null)
    {
        var statusCodeOperation = _notificationServices.StatusCode;

        ICommandResult result = default;

        if (_notificationServices.HasNotifications())
        {
            result = CreateErrorResponse(commandResult, statusCodeOperation.Id);

            _notificationServices.ClearNotifications();
        }


        switch (statusCodeOperation)
        {
            case var _ when statusCodeOperation == StatusCodeOperation.BadRequest:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.NotFound);
                var badRequestResponse = requestData.CreateResponse();
                await badRequestResponse.WriteAsJsonAsync(commandResult);
                badRequestResponse.StatusCode = HttpStatusCode.BadRequest;
                return badRequestResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.NotFound:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.NotFound);
                var notFoundResponse = requestData.CreateResponse();
                await notFoundResponse.WriteAsJsonAsync(commandResult);
                notFoundResponse.StatusCode = HttpStatusCode.NotFound;
                return notFoundResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.BusinessError:
                GenerateLogResponse(commandResult, (int)HttpStatusCode.UnprocessableEntity);
                var unprocessableEntityResponse = requestData.CreateResponse();
                await unprocessableEntityResponse.WriteAsJsonAsync(commandResult);
                unprocessableEntityResponse.StatusCode = HttpStatusCode.UnprocessableEntity;
                return unprocessableEntityResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.Created:
                var createdResponse = requestData.CreateResponse();
                await createdResponse.WriteAsJsonAsync(commandResult);
                createdResponse.StatusCode = HttpStatusCode.Created;
                return createdResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.NoContent:
                var noContentResponse = requestData.CreateResponse();
                noContentResponse.StatusCode = HttpStatusCode.NoContent;
                return noContentResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.OK:
                var okResponse = requestData.CreateResponse();
                await okResponse.WriteAsJsonAsync(commandResult);
                okResponse.StatusCode = HttpStatusCode.OK;
                return okResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.Accepted:
                var acceptedResponse = requestData.CreateResponse();
                await acceptedResponse.WriteAsJsonAsync(commandResult);
                acceptedResponse.StatusCode = HttpStatusCode.Accepted;
                return acceptedResponse;
            case var _ when statusCodeOperation == StatusCodeOperation.Found:
                var foundResponse = requestData.CreateResponse();
                await foundResponse.WriteAsJsonAsync(commandResult);
                foundResponse.StatusCode = HttpStatusCode.Found;
                return foundResponse;
            default:
                var defaultResponse = requestData.CreateResponse();
                await defaultResponse.WriteAsJsonAsync(commandResult);
                defaultResponse.StatusCode = HttpStatusCode.OK;
                return defaultResponse;
        }
    }

    public void GenerateLogResponse(CommandResult commandResult, int statusCode, bool hasError = true)
    {
        _logServices.LogData.AddResponseStatusCode(statusCode)
                            .AddResponseBody(commandResult);

        if (hasError)
            _logServices.WriteErrorLog();
        else
            _logServices.WriteLog();
    }

    public ICommandResult CreateErrorResponse(CommandResult commandResult, int statusCode)
    {
        var options = JsonOptionsFactory.GetSerializerOptions();

        var notifications = _notificationServices.GetNotifications();

        var jsonNotifications = JsonSerializer.Serialize(notifications, options);

        var invalidParams = jsonNotifications;
        var defaultTitle = "Um erro ocorreu ao processar o request.";

        var problemDetails = new FunctionProblemDetail(invalidParams, commandResult.Message, statusCode, defaultTitle);

        commandResult.Data = problemDetails;

        return commandResult;
    }

}
