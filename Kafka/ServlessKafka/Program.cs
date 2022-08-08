using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serverless.Extensions.Logs;
using Serverless.Extensions.Options;
using ServerlessKafka.Middlewares;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(
    builder =>
    {
        builder.UseMiddleware<ExceptionLoggingMiddleware>();
    })
    .ConfigureServices((hostContext, services) =>
    {
        //Recuperando o environment e as configurações
        var env = hostContext.HostingEnvironment;
        var config = GetConfiguration(args, env);

        //add serilog
        Log.Logger = LogIntegrationsExtensions.CreateFunctionLogWithSerilog(config);

        services.AddLogging(lb => lb.ClearProviders().AddSerilog(Log.Logger, true));
        services.AddFilterToSystemLogs()
                .AddLogServiceDependencies()
                .AddBaseConfigurationOptionsPattern(config);
    })
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
