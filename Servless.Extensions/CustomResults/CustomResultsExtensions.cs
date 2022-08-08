using Microsoft.Extensions.DependencyInjection;

namespace Serverless.Extensions.CustomResults;

public static class CustomResultsExtensions
{
    public static IServiceCollection AddApiCustomResults(this IServiceCollection services)
    {
        services.AddSingleton<IApiCustomResults, ApiCustomResults>();
        return services;
    }
}
