using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases;
using Acompanhamento.UseCases.DTOs;
using Acompanhamento.UseCases.Gateway;
using Moq;

namespace Acompanhamento.BDD.Tests.StepDefinitions
{
    [Binding]
    public class AcompanhamentoStepDefinitions
    {
        private AcompanhamentoUseCases _acompanhamentoUseCases { get; set; }
        private PedidoDto pedido;

        private Mock<IAcompanhamentoPersistenceGateway> _acompanhamentoPersistantGateway;

        [BeforeScenario("acompanhamento")]
        public void BeforeScenarioWithTag ()
        {
            _acompanhamentoPersistantGateway = new Mock<IAcompanhamentoPersistenceGateway>();

            _acompanhamentoPersistantGateway
                .Setup(x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()))
                .ReturnsAsync(true);

            _acompanhamentoUseCases = new AcompanhamentoUseCases(_acompanhamentoPersistantGateway.Object);
        }

        [Given("the order to be saved")]
        public void GivenTheOrderToBeSaved()
        {
            pedido = new PedidoDto()
            {
                IdPedido = Guid.NewGuid(),
                ClienteName = "Bruna Pisera"
            };
        }

        [Given("the order does not exists on the database")]
        public async Task GivenTheOrderDoesNotExistsOnTheDatabase()
        {
            await _acompanhamentoUseCases.SalvarPedidoComoRecebidoAsync(pedido);
        }

        [Then("should save the order on the database with the status Recebido")]
        public void ThenShouldSaveTheOrderOnTheDatabaseWithTheStatusRecebido()
        {
            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.Is<AcompanhamentoAggregate>(a =>
                    a.Status == Status.Recebido &&
                    a.IdPedido == pedido.IdPedido &&
                    a.ClientName == pedido.ClienteName
                )),
                Times.Once
            );
        }
    }
}
