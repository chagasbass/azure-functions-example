using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Serverless.Extensions.Documentations;

public static class SwaggerExtensions
{
    const string mensagemPadrao = "Não informado";
    const string urlPadrao = @"https://www.restoque.com.br/";

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        #region preenchendo informações da documentação

        var applicationName = configuration["BaseConfiguration:NomeAplicacao"];
        var applicationDescription = configuration["BaseConfiguration:Descricao"];
        var developerName = configuration["BaseConfiguration:Desenvolvedor"];
        var companyName = configuration["BaseConfiguration:NomeEmpresa"];
        var companyUrl = configuration["BaseConfiguration:UrlEmpresa"];
        var hasAuthentication = bool.Parse(configuration["BaseConfiguration:TemAutenticacao"]);

        if (string.IsNullOrEmpty(companyUrl))
            companyUrl = urlPadrao;

        if (string.IsNullOrEmpty(companyName))
            companyName = mensagemPadrao;

        var uri = new Uri(companyUrl);

        var info = new OpenApiInfo
        {
            Title = applicationName,
            Description = $"{applicationDescription} Developed by {developerName}",
            License = new OpenApiLicense { Name = companyName, Url = uri }
        };

        #endregion

        services.AddSwaggerGen(delegate (SwaggerGenOptions c)
        {
            #region Resolver conflitos de rotas
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            #endregion

            c.SwaggerDoc("v1", info);

            #region Inserindo Autenticação Bearer no swagger

            if (hasAuthentication)
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "Autorização efetuada via JWT token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };

                c.AddSecurityRequirement(securityRequirement);
            }

            #endregion
        });



        return services;
    }
}
