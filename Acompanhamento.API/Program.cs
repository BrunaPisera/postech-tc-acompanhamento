using Acompanhamento.Infrastructure;
using Acompanhamento.Infrastructure.Broker;
using Acompanhamento.UseCases.DTOs;
using Acompanhamento.UseCases.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DI
builder.Services.AddInfrastructure();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Adiciona o health check na rota "/health"
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    services.EnsureDatabaseMigrated();      

    var rabbitMqConnection = new BrokerConnection();
    var rabbitMqConsumer = new BrokerConsumer(rabbitMqConnection);

    rabbitMqConsumer.BrokerStartConsumer<PedidoDto>(
        queueName: "savePedido",
        exchange: "pedidosOperations",
        routingKey: "pedidoRealizado",
        callback: async (pedido) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var acompanhamentoUseCases = scopedServices.GetRequiredService<IAcompanhamentoUseCases>();         
                await acompanhamentoUseCases.SalvarPedidoComoRecebidoAsync(pedido);
            }
        });

    rabbitMqConsumer.BrokerStartConsumer<PedidoDto>(
        queueName: "confirmaPagamento",
        exchange: "pedidosOperations",
        routingKey: "pagamentoRealizado",
        callback: async (pedido) => {
            using (var innerScope = app.Services.CreateScope())
            {
                var scopedServices = innerScope.ServiceProvider;
                var acompanhamentoUseCases = scopedServices.GetRequiredService<IAcompanhamentoUseCases>();
                await acompanhamentoUseCases.AtualizaStatusComoEmPreparacaoAsync(pedido);
            }
        });
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
