using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serverless.Extensions.Shared.Logs;
using Serverless.Extensions.Shared.Logs.Services;
using Serverless.Extensions.Shared.Notifications;

namespace Serverless.Extensions.Logs;

public static class LogIntegrationsExtensions
{
    public static Logger ConfigureStructuralLogWithSerilog(IConfiguration configuration)
    {
        var instrumentationKey = configuration["Values:APPINSIGHTS_INSTRUMENTATIONKEY"];
        var functionName = configuration["BaseConfiguration:FunctionName"];

        var telemetryConfiguration = new TelemetryConfiguration(instrumentationKey);

        return new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
        .MinimumLevel.Override("System", LogEventLevel.Error)
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/index.html")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/index.html")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/v1/swagger.json")))
        .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/v1/swagger.json")))
        .Destructure.ByTransforming<HttpRequest>(x => new
        {
            x.Method,
            Url = x.Path,
            x.QueryString
        })
        .Enrich.WithProperty("Application", functionName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
        .CreateLogger();
    }

    public static Logger CreateFunctionLogWithSerilog(IConfiguration configuration)
    {
        var instrumentationKey = configuration["Values:APPINSIGHTS_INSTRUMENTATIONKEY"];
        var functionName = configuration["BaseConfiguration:FunctionName"];

        var telemetryConfiguration = new TelemetryConfiguration(instrumentationKey);

        return new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Error)
        .MinimumLevel.Override("System", LogEventLevel.Error)
        .Enrich.WithProperty("Application", functionName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
        .CreateLogger();
    }

    public static IServiceCollection AddFilterToSystemLogs(this IServiceCollection services)
    {
        services.AddLogging(builder =>
        {

            builder.AddFilter("Microsoft", LogLevel.Warning)
                   .AddFilter("System", LogLevel.Warning)
                   .AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Warning)
                   .AddFilter("Microsoft.AspNetCore", LogLevel.Warning)
                   .AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning)
                   .AddConsole();
        });

        return services;
    }

    public static IServiceCollection AddLogServiceDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ILogServices, LogServices>();
        services.AddSingleton<LogData>();

        return services;
    }

    public static IServiceCollection AddNotificationControl(this IServiceCollection services)
    {
        services.AddSingleton<INotificationServices, NotificationServices>();
        return services;
    }
}
