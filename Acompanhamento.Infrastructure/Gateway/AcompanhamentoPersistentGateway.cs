using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.Infrastructure.Data;
using Acompanhamento.UseCases.Gateway;
using Microsoft.EntityFrameworkCore;

namespace Acompanhamento.Infrastructure.Gateway
{
    internal class AcompanhamentoPersistentGateway : IAcompanhamentoPersistenceGateway
    {
        private ApplicationContext Context;

        public AcompanhamentoPersistentGateway(ApplicationContext context)
        {
            Context = context;
        }
        public async Task<List<AcompanhamentoAggregate>> GetAllPedidosByStatusAsync(Status status)
        {
            return await Context.Acompanhamento
                        .Include(x => x.CodigoAcompanhamento)
                        .Include(x => x.ClientName)
                        .Include(x => x.IdPedido)                                            
                        .Where(x => x.Status == status)
                        .ToListAsync();
        }

        public async Task<List<AcompanhamentoAggregate>> GetAllPedidosNaoFinalizadosAsync()
        {
            return await Context.Acompanhamento
                        .Include(x => x.CodigoAcompanhamento)
                        .Include(x => x.ClientName)
                        .Include(x => x.IdPedido)
                        .Where(x => x.Status != Status.Finalizado)
                        .ToListAsync();
        }

        public async Task<AcompanhamentoAggregate?> GetPedidoById(string idPedido)
        {
            return await Context.Acompanhamento
                       .Include(x => x.CodigoAcompanhamento)
                       .Include(x => x.ClientName)
                       .Include(x => x.IdPedido)
                       .FirstOrDefaultAsync(x => x.IdPedido == idPedido);
        }

        public Task<bool> SaveAcompanhamentoAsync(AcompanhamentoAggregate acompanhamento)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SavePagamentoAsync(AcompanhamentoAggregate pagamento)
        {
            throw new NotImplementedException();
        }
    }
}
