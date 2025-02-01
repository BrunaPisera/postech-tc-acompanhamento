using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.DTOs;
using Acompanhamento.UseCases.Exceptions;
using Acompanhamento.UseCases.Extensions;
using Acompanhamento.UseCases.Gateway;
using Acompanhamento.UseCases.Interfaces;

namespace Acompanhamento.UseCases
{
    public class AcompanhamentoUseCases : IAcompanhamentoUseCases
    {
        private readonly IAcompanhamentoPersistenceGateway AcompanhamentoPersistencePort;

        public AcompanhamentoUseCases(IAcompanhamentoPersistenceGateway acompanhamentoPersistencePort)
        {
            AcompanhamentoPersistencePort = acompanhamentoPersistencePort;
        }

        public async Task SalvarPedidoComoRecebidoAsync(PedidoDto pedidoDto)
        {               
            var pedidoExistente = await TryGetPedidoById(pedidoDto.IdPedido);

            if (pedidoExistente != null)
            {                
                Console.WriteLine($"Pedido {pedidoDto.IdPedido} já existe. Não será inserido novamente.");
                return;
            }

            var acompanhamento = new AcompanhamentoAggregate()
            {
                Status = Status.Recebido,
                IdPedido = pedidoDto.IdPedido,
                ClientName = pedidoDto.ClienteName,
            };
            await TryToSaveAcompanhamento(acompanhamento);
        }

        public async Task AtualizaStatusComoEmPreparacaoAsync(PedidoDto pedidoDto)
        {
            var pedido = await TryGetPedidoById(pedidoDto.IdPedido);

            if (pedido.Status != Status.Recebido)
                throw new OperacaoInvalidaException("Status do pedido precisa estar como recebido para ser atualizado como em preparação.");

            pedido.Status = Status.Preparacao;

            await TryToSaveAcompanhamento(pedido);
        }

        public async Task AtualizaStatusComoProntoAsync(Guid idPedido)
        {
            var pedido = await TryGetPedidoById(idPedido);

            if (pedido.Status != Status.Preparacao)
                throw new OperacaoInvalidaException("Status do pedido precisa estar em preparação para ser atualizado como pronto.");

            pedido.Status = Status.Pronto;

            await TryToSaveAcompanhamento(pedido);
        }          


        public async Task FinalizaPedidoAsync(Guid idPedido)
        {
            var pedido = await TryGetPedidoById(idPedido);

            if (pedido.Status != Status.Pronto)
                throw new OperacaoInvalidaException("Status do pedido precisa estar como pronto para que o pedido possa ser finalizado.");

            pedido.Status = Status.Finalizado;

            await TryToSaveAcompanhamento(pedido);
        }

        public async Task<List<AcompanhamentoDto>> GetAllPedidosAsync()
        {
            var pedidos = await AcompanhamentoPersistencePort.GetAllPedidosNaoFinalizadosAsync();
            var pedidosDto = pedidos.Select(x => x.ToPedidoDto());

            return pedidosDto.ToList();
        }

        public async Task<List<AcompanhamentoDto>> GetAllPedidosByStatusAsync(Status status)
        {
            var acompanhamentos = await AcompanhamentoPersistencePort.GetAllPedidosByStatusAsync(status);
            var acompanhamentosDto = acompanhamentos.Select(x => x.ToPedidoDto());

            return acompanhamentosDto.ToList();
        }

        private async Task<AcompanhamentoAggregate?> TryGetPedidoById(Guid idPedido)
        {
            var pedido = await AcompanhamentoPersistencePort.GetAcompanhamentoByPedidoIdAsync(idPedido);

            return pedido;
        }

        private async Task TryToSaveAcompanhamento(AcompanhamentoAggregate? acompanhamento)
        {
            var acompanhamentoAtualizado = await AcompanhamentoPersistencePort.SaveAcompanhamentoAsync(acompanhamento);

            if (!acompanhamentoAtualizado) throw new ConfirmarPagamentoException("Nao foi possivel atualizar o status do pedido.");
        }
    }
}
