using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Serverless.Extensions.Shared.Logs.Services;

namespace ServerlessKafka.Middlewares;

public class ExceptionLoggingMiddleware : IFunctionsWorkerMiddleware
{
    private readonly ILogServices _logServices;

    public ExceptionLoggingMiddleware(ILogServices logServices)
    {
        _logServices = logServices;
    }

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            /*TODO: Analisar pra pegar alguns dados dentro do context.
             * pegar o nome do topico
             * pegar o id da function
             * pegar o trace da function
             */

            var data = context.BindingContext;
            var otherData = data.BindingData;

            await next(context);

            var data2 = context.BindingContext;
            var otherData2 = data.BindingData;
        }
        catch (Exception ex)
        {
            _logServices.LogData.AddException(ex);
            _logServices.WriteMessage("Erro na operação");
            _logServices.WriteLogWhenRaiseExceptions();
        }
    }
}
