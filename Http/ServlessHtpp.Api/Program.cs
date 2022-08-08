using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serverless.Extensions.CustomResults;
using Serverless.Extensions.Logs;
using Serverless.Extensions.Options;
using ServerlessHtpp.Api.Extensions;
using ServerlessHttp.Api.Middlewares;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(
    builder =>
    {
        builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        //builder.UseMidleware<SerilogRequestLoggerMiddleware>();

    })
    .ConfigureServices((hostContext, services) =>
    {
        //Recuperando o environment e as configurações
        var env = hostContext.HostingEnvironment;
        var config = GetConfiguration(args, env);

        //add serilog
        Log.Logger = LogIntegrationsExtensions.ConfigureStructuralLogWithSerilog(config);

        services.AddLogging(lb => lb.ClearProviders().AddSerilog(Log.Logger, true));
        services.AddDependecyInjectionsFunction()
                .AddFilterToSystemLogs()
                .AddNotificationControl()
                .AddLogServiceDependencies()
                .AddApiCustomResults()
                .AddFilterToSystemLogs()
                .AddGlobalExceptionHandlerMiddleware()
                .AddBaseConfigurationOptionsPattern(config);
    })
    .ConfigureOpenApi()
    .Build();

host.Run();

static IConfiguration GetConfiguration(string[] args, IHostEnvironment environment)
{
    return new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
       .AddJsonFile("host.json", optional: false, reloadOnChange: true)
       .AddUserSecrets<Program>(optional: true, reloadOnChange: true)
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .Build();
}

