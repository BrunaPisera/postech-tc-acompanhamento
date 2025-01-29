using Acompanhamento.Infrastructure.Data;
using Acompanhamento.UseCases.Interfaces;
using Acompanhamento.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace Acompanhamento.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {            
            services.AddScoped<IAcompanhamentoUseCases, AcompanhamentoUseCases>();

            services.AddDbContext<ApplicationContext>();

            return services;
        }
    }
}
