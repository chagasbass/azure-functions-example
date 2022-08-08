using Microsoft.Extensions.DependencyInjection;
using ServerlessHtpp.Api.Core.Application.Interfaces;
using ServerlessHtpp.Api.Core.Application.Services;
using ServerlessHtpp.Api.Core.Domain.Repositories;
using ServerlessHtpp.Api.Core.Infra.Data.Contexts;
using ServerlessHtpp.Api.Core.Infra.Data.Repositories;
using ServerlessHtpp.Api.Core.Infra.Messaging.Interfaces;
using ServerlessHtpp.Api.Core.Infra.Messaging.Services;

namespace ServerlessHtpp.Api.Extensions
{
    public static class FunctionDIExtensions
    {
        public static IServiceCollection AddDependecyInjectionsFunction(this IServiceCollection services)
        {
            services.AddScoped<ProdutoDataContext, ProdutoDataContext>();
            services.AddScoped<IProdutoDataContext, ProdutoDataContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoMessageServices, ProdutoMessageServices>();
            services.AddScoped<IProdutoApplicationServices, ProdutoApplicationServices>();

            return services;
        }
    }
}
