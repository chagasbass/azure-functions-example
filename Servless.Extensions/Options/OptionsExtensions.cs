using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serverless.Extensions.Shared.Configurations;

namespace Serverless.Extensions.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
        services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.BaseConfig));
        services.Configure<MessagingConfigurationOptions>(configuration.GetSection(MessagingConfigurationOptions.MessagingConfig));
        return services;
    }
}
