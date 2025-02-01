using Acompanhamento.Core.Entities;
using Acompanhamento.UseCases.DTOs;

namespace Acompanhamento.UseCases.Extensions
{
    static internal class PedidoAggregateExtensions
    {
        static internal AcompanhamentoDto ToPedidoDto(this AcompanhamentoAggregate acompanhamentoAggregate)
        {
            var pedido = new AcompanhamentoDto()
            {
                CodigoAcompanhamento = acompanhamentoAggregate.CodigoAcompanhamento,
                Status = acompanhamentoAggregate.Status,
                IdPedido = acompanhamentoAggregate.IdPedido,
                ClientName = acompanhamentoAggregate.ClientName,
            };       

            return pedido;
        }
    }
}
