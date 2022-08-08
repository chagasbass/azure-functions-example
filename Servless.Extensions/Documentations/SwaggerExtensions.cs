using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Serverless.Extensions.Documentations;

public static class SwaggerExtensions
{
    const string mensagemPadrao = "Não informado";
    const string urlPadrao = @"https://www.restoque.com.br/";

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {





        //services.AddSwaggerGen(delegate (SwaggerGenOptions c)
        //{
        //    #region Resolver conflitos de rotas
        //    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        //    #endregion

        //    c.SwaggerDoc("v1", info);

        //    #region Inserindo Autenticação Bearer no swagger

        //    if (hasAuthentication)
        //    {
        //        var securitySchema = new OpenApiSecurityScheme
        //        {
        //            Description = "Autorização efetuada via JWT token",
        //            Name = "Authorization",
        //            In = ParameterLocation.Header,
        //            Type = SecuritySchemeType.Http,
        //            Scheme = "bearer",
        //            Reference = new OpenApiReference
        //            {
        //                Type = ReferenceType.SecurityScheme,
        //                Id = "Bearer"
        //            }
        //        };

        //        c.AddSecurityDefinition("Bearer", securitySchema);

        //        var securityRequirement = new OpenApiSecurityRequirement
        //        {
        //            { securitySchema, new[] { "Bearer" } }
        //        };

        //        c.AddSecurityRequirement(securityRequirement);
        //    }

        //    #endregion
        //});



        return services;
    }
}
