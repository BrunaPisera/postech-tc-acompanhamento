using Acompanhamento.Core.Entities.Enums;

namespace Acompanhamento.UseCases.DTOs
{
    public class AcompanhamentoDto
    {
        public short CodigoAcompanhamento { get; set; }
        public Status Status { get; set; }
        public Guid IdPedido { get; set; }
        public string ClientName { get; set; }
    }
}