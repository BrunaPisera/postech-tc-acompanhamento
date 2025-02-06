using Acompanhamento.Core.Entities;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.DTOs;
using Acompanhamento.UseCases.Exceptions;
using Acompanhamento.UseCases.Gateway;
using Moq;

namespace Acompanhamento.UseCases.Tests
{
    public class AcompanhamentoUseCasesTests
    {
        private AcompanhamentoUseCases _acompanhamentoUseCases {  get; set; }

        private Mock<IAcompanhamentoPersistenceGateway> _acompanhamentoPersistantGateway;
        
        [SetUp]
        public void Setup()
        {
            _acompanhamentoPersistantGateway = new Mock<IAcompanhamentoPersistenceGateway>();

            _acompanhamentoPersistantGateway
                .Setup(x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()))
                .ReturnsAsync(true);

            _acompanhamentoUseCases = new AcompanhamentoUseCases(_acompanhamentoPersistantGateway.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_acompanhamentoUseCases, Is.Not.Null);
        }

        [Test]
        public async Task Should_Save_Pedido_When_Pedido_Does_Not_Exists()
        {
            var pedido = new PedidoDto()
            {
                IdPedido = Guid.NewGuid(),
                ClienteName = "Bruna Pisera"
            };    

            await _acompanhamentoUseCases.SalvarPedidoComoRecebidoAsync(pedido);

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.Is<AcompanhamentoAggregate>(a =>
                    a.Status == Status.Recebido &&
                    a.IdPedido == pedido.IdPedido &&
                    a.ClientName == pedido.ClienteName 
                )),
                Times.Once
            );
        }

        [Test]
        public async Task Should_Not_Save_Pedido_When_Pedido_Already_Exists()
        {
            _acompanhamentoPersistantGateway
                .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new AcompanhamentoAggregate { 
                    IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                    ClientName = "Teste" 
                });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.NewGuid(),
                ClienteName = "Bruna Pisera"
            };

            await _acompanhamentoUseCases.SalvarPedidoComoRecebidoAsync(pedido);

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()),
                Times.Never);
        }

        [Test]
        public async Task Should_Update_Pedido_Status_To_EmPreparacao()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Recebido
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            await _acompanhamentoUseCases.AtualizaStatusComoEmPreparacaoAsync(pedido);

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.Is<AcompanhamentoAggregate>(a =>
                    a.Status == Status.Preparacao &&
                    a.IdPedido == pedido.IdPedido &&
                    a.ClientName == pedido.ClienteName
                )),
                Times.Once
            );
        }

        [Test]
        public void Should_Not_Update_Pedido_Status_To_EmPreparacao_If_Pedido_Is_Not_Recebido()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Preparacao
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            Assert.ThrowsAsync<OperacaoInvalidaException>(async () =>
                await _acompanhamentoUseCases.AtualizaStatusComoEmPreparacaoAsync(pedido)
            );

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()),
                Times.Never);
        }

        [Test]
        public async Task Should_Update_Pedido_Status_To_Pronto()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Preparacao
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            await _acompanhamentoUseCases.AtualizaStatusComoProntoAsync(pedido.IdPedido);

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.Is<AcompanhamentoAggregate>(a =>
                    a.Status == Status.Pronto &&
                    a.IdPedido == pedido.IdPedido &&
                    a.ClientName == pedido.ClienteName
                )),
                Times.Once
            );
        }

        [Test]
        public async Task Should_Not_Update_Pedido_Status_To_Recebido_If_Pedido_Is_Not_EmPreparacao()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Recebido
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            Assert.ThrowsAsync<OperacaoInvalidaException>(async () =>
                await _acompanhamentoUseCases.AtualizaStatusComoProntoAsync(pedido.IdPedido)
            );

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()),
                Times.Never);
        }

        [Test]
        public async Task Should_Update_Pedido_Status_To_Finalizado()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Pronto
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            await _acompanhamentoUseCases.FinalizaPedidoAsync(pedido.IdPedido);

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.Is<AcompanhamentoAggregate>(a =>
                    a.Status == Status.Finalizado &&
                    a.IdPedido == pedido.IdPedido &&
                    a.ClientName == pedido.ClienteName
                )),
                Times.Once
            );
        }

        [Test]
        public async Task Should_Not_Update_Pedido_Status_To_Finalizado_If_Pedido_Is_Not_Pronto()
        {
            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAcompanhamentoByPedidoIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(new AcompanhamentoAggregate
               {
                   IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                   ClientName = "Bruna Pisera",
                   Status = Status.Preparacao
               });

            var pedido = new PedidoDto()
            {
                IdPedido = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
                ClienteName = "Bruna Pisera"
            };

            Assert.ThrowsAsync<OperacaoInvalidaException>(async () =>
                await _acompanhamentoUseCases.FinalizaPedidoAsync(pedido.IdPedido)
            );

            _acompanhamentoPersistantGateway.Verify(
                x => x.SaveAcompanhamentoAsync(It.IsAny<AcompanhamentoAggregate>()),
                Times.Never);
        }

        [Test]
        public async Task Should_Get_Pedido_ByStatus()
        {
            var status = Status.Recebido;
            var pedidosMock = new List<AcompanhamentoAggregate>
            {
                new AcompanhamentoAggregate { IdPedido = Guid.NewGuid(), ClientName = "Cliente 1", Status = status },
                new AcompanhamentoAggregate { IdPedido = Guid.NewGuid(), ClientName = "Cliente 2", Status = status },
            };

            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAllPedidosByStatusAsync(status))
               .ReturnsAsync(pedidosMock);

            var result = await _acompanhamentoUseCases.GetAllPedidosByStatusAsync(status);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(pedidosMock.Count));
            Assert.That(result.Select(x => x.IdPedido), Is.EquivalentTo(pedidosMock.Select(x => x.IdPedido)));
            Assert.That(result.Select(x => x.ClientName), Is.EquivalentTo(pedidosMock.Select(x => x.ClientName)));

            _acompanhamentoPersistantGateway.Verify(x => x.GetAllPedidosByStatusAsync(status), Times.Once);
        }

        [Test]
        public async Task Should_Get_All_Pedido_Not_Finalizados()
        {
            var status = Status.Preparacao;
            var pedidosMock = new List<AcompanhamentoAggregate>
            {
                new AcompanhamentoAggregate { IdPedido = Guid.NewGuid(), ClientName = "Cliente 1", Status = status },
                new AcompanhamentoAggregate { IdPedido = Guid.NewGuid(), ClientName = "Cliente 2", Status = status },
            };

            _acompanhamentoPersistantGateway
               .Setup(x => x.GetAllPedidosNaoFinalizadosAsync())
               .ReturnsAsync(pedidosMock);

            var result = await _acompanhamentoUseCases.GetAllPedidosAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(pedidosMock.Count));
            Assert.That(result.Select(x => x.IdPedido), Is.EquivalentTo(pedidosMock.Select(x => x.IdPedido)));
            Assert.That(result.Select(x => x.ClientName), Is.EquivalentTo(pedidosMock.Select(x => x.ClientName)));

            _acompanhamentoPersistantGateway.Verify(x => x.GetAllPedidosNaoFinalizadosAsync(), Times.Once);
        }
    }
}