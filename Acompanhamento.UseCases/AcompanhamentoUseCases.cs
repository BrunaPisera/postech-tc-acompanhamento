using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.Exceptions;
using Acompanhamento.UseCases.Gateway;
using Acompanhamento.UseCases.Interfaces;

namespace Acompanhamento.UseCases
{
    public class AcompanhamentoUseCases : IAcompanhamentoUseCases
    {
        private readonly IAcompanhamentoPersistenceGateway AcompanhamentoPersistencePort;

        public AcompanhamentoUseCases(IAcompanhamentoPersistenceGateway pedidoPersistencePort)
        {
            AcompanhamentoPersistencePort = pedidoPersistencePort;
        }

        public Task AtualizaStatusComoEmPreparacaoAsync(Guid idPedido)
        {
            throw new NotImplementedException();
        }

        public async Task AtualizaStatusComoProntoAsync(string idPedido)
        {
            var pedido = await TryGetPedidoById(idPedido);

            if (pedido.Status != Status.Preparacao)
                throw new OperacaoInvalidaException("Status do pedido precisa estar em preparação para ser atualizado como pronto.");

            pedido.Status = Status.Pronto;

            await TryToSaveAcompanhamento(pedido);
        }

        public Task AtualizaStatusComoProntoAsync(Guid idPedido)
        {
            throw new NotImplementedException();
        }

        public Task FinalizaPedidoAsync(Guid idPedido)
        {
            throw new NotImplementedException();
        }

        public Task SalvarPedidoComoRecebidoAsync(Guid idPedido)
        {
            throw new NotImplementedException();
        }

        private async Task<AcompanhamentoAggregate?> TryGetPedidoById(string idPedido)
        {
            var pedido = await AcompanhamentoPersistencePort.GetAcompanhamentoByPedidoIdAsync(idPedido);

            if (pedido == null) throw new PedidoNaoEncontradoException("Pedido nao encontrado");

            return pedido;
        }

        private async Task TryToSaveAcompanhamento(AcompanhamentoAggregate? acompanhamento)
        {
            var acompanhamentoAtualizado = await AcompanhamentoPersistencePort.SaveAcompanhamentoAsync(acompanhamento);

            if (!acompanhamentoAtualizado) throw new ConfirmarPagamentoException("Nao foi possivel atualizar o status do pedido.");
        }
    }
}
