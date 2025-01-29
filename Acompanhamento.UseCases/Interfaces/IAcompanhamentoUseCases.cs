﻿namespace Acompanhamento.UseCases.Interfaces
{
    public interface IAcompanhamentoUseCases
    {   
        Task AtualizaStatusComoProntoAsync(Guid idPedido);           
        Task FinalizaPedidoAsync(Guid idPedido);
    }
}
