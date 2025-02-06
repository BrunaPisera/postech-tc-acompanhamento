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
                  .Where(x => x.Status == status)
                  .ToListAsync();
        }

        public async Task<List<AcompanhamentoAggregate>> GetAllPedidosNaoFinalizadosAsync()
        {
            return await Context.Acompanhamento                       
                        .Where(x => x.Status != Status.Finalizado)
                        .ToListAsync();
        }

        public async Task<AcompanhamentoAggregate?> GetAcompanhamentoByPedidoIdAsync(Guid idPedido)
        {
            return await Context.Acompanhamento                     
                       .FirstOrDefaultAsync(x => x.IdPedido == idPedido);
        }

        public async Task<bool> SaveAcompanhamentoAsync(AcompanhamentoAggregate acompanhamento)
        {
            Context.Acompanhamento.Update(acompanhamento);

            var result = await Context.SaveChangesAsync();

            return result > 0;
        }
    }
}
