using Acompanhamento.Infrastructure.Data;
using Acompanhamento.UseCases.Interfaces;
using Acompanhamento.UseCases;
using Microsoft.Extensions.DependencyInjection;
using Acompanhamento.UseCases.Gateway;
using Acompanhamento.Infrastructure.Gateway;
using Acompanhamento.Infrastructure.Broker;

namespace Acompanhamento.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {            
            services.AddScoped<IAcompanhamentoUseCases, AcompanhamentoUseCases>();
            services.AddScoped<IAcompanhamentoPersistenceGateway, AcompanhamentoPersistentGateway>();
            services.AddScoped<IBrokerConnection, BrokerConnection>(); 
            services.AddScoped<BrokerConsumer>();

            services.AddDbContext<ApplicationContext>();

            return services;
        }
    }
}
