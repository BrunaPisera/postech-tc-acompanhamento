using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.Core.Entities;

namespace Acompanhamento.UseCases.Gateway
{
    public interface IAcompanhamentoPersistenceGateway
    {     
        Task<List<AcompanhamentoAggregate>> GetAllPedidosNaoFinalizadosAsync();
        Task<AcompanhamentoAggregate> GetAcompanhamentoByPedidoIdAsync(Guid pedidoId);
        Task<bool> SaveAcompanhamentoAsync(AcompanhamentoAggregate acompanhamento);
        Task<List<AcompanhamentoAggregate>> GetAllPedidosByStatusAsync(Status status);
    }
}