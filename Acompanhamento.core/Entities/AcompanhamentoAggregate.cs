using Acompanhamento.Core.Entities.Enums;

namespace Acompanhamento.Core.Entities
{
    public class AcompanhamentoAggregate : Entity<Guid>, IAggregateRoot
    {
        public short CodigoAcompanhamento { get; set; }
        public Status Status { get; set; }
        public string IdPedido { get; set; }
        public string ClientName { get; set; }
    }
}
