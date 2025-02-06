using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.DTOs;

namespace Acompanhamento.UseCases.Interfaces
{
    public interface IAcompanhamentoUseCases
    {
        Task SalvarPedidoComoRecebidoAsync(PedidoDto pedido);
        Task AtualizaStatusComoEmPreparacaoAsync(PedidoDto pedido);
        Task AtualizaStatusComoProntoAsync(Guid idPedido);
        Task FinalizaPedidoAsync(Guid idPedido);
        Task<List<AcompanhamentoDto>> GetAllPedidosAsync();
        Task<List<AcompanhamentoDto>> GetAllPedidosByStatusAsync(Status status);
    }
}
