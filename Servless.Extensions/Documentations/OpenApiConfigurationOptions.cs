using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Serverless.Extensions.Documentations;

public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
{
    public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
    {
        Version = GetOpenApiDocVersion(),
        Title = GetOpenApiDocTitle(),
        Description = "Azure Function usando Http Trigger",
        TermsOfService = new Uri("https://www.restoque.com.br/"),
        Contact = new OpenApiContact()
        {
            Name = "Thiago Chagas",
            Email = "thiago.chagas@restoque.com",
            Url = new Uri("https://www.restoque.com.br/"),
        },
        License = new OpenApiLicense()
        {
            Name = "MIT",
            Url = new Uri("http://opensource.org/licenses/MIT"),
        }
    };

    public override OpenApiVersionType OpenApiVersion { get; set; } = GetOpenApiVersion();
}
