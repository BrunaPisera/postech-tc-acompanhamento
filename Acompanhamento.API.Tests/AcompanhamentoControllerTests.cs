using Acompanhamento.API.Controllers;
using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.DTOs;
using Acompanhamento.UseCases.Exceptions;
using Acompanhamento.UseCases.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Acompanhamento.API.Tests
{
    public class AcompanhamentoControllerTests
    {
        private AcompanhamentoController _controller;

        private Mock<IAcompanhamentoUseCases> _acompanhamentoUseCasesMock;

        [SetUp]
        public void Setup()
        {
            _acompanhamentoUseCasesMock = new Mock<IAcompanhamentoUseCases>();
            _controller = new AcompanhamentoController(_acompanhamentoUseCasesMock.Object);
        }

        [Test]
        public void Can_Create()
        {
            Assert.That(_controller, Is.Not.Null);
        }

        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsOk_WhenSuccessful()
        {        
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock.Setup(x => x.AtualizaStatusComoProntoAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
        
            var result = await _controller.AtualizaStatusComoPronto(idPedido);
                    
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsBadRequest_WhenIdIsInvalid()
        {        
            var idPedido = "invalid-guid";
                     
            var result = await _controller.AtualizaStatusComoPronto(idPedido);
                      
            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.Value, Is.EqualTo("O id do pagamento nao pode ser nulo."));
        }

        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsNotFound_WhenPedidoNaoEncontradoExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock.Setup(x => x.AtualizaStatusComoProntoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new PedidoNaoEncontradoException("Pedido nao encontrado"));

            var result = await _controller.AtualizaStatusComoPronto(idPedido);

            var notFoundResult = result as NotFoundObjectResult;

            Assert.That(notFoundResult, Is.Not.Null);
        }

        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsInternalServerError_WhenAcompanhamentoNaoEncontradoExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.AtualizaStatusComoProntoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new AcompanhamentoNaoEncontradoException("Acompanhamento nao encontrado"));

            var result = await _controller.AtualizaStatusComoPronto(idPedido);

            var objectResult = result as ObjectResult;
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }


        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsBadRequest_WhenOperacaoInvalidaExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock.Setup(x => x.AtualizaStatusComoProntoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new OperacaoInvalidaException("Operacao invalida"));

            var result = await _controller.AtualizaStatusComoPronto(idPedido);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
        }

        [Test]
        public async Task AtualizaStatusComoPronto_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock.Setup(x => x.AtualizaStatusComoProntoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _controller.AtualizaStatusComoPronto(idPedido);

            var serverErrorResult = result as ObjectResult;
            Assert.That(serverErrorResult, Is.Not.Null);
            Assert.That(serverErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task Finaliza_ReturnsBadRequest_WhenIdIsInvalid()
        {
            var idPedido = "invalid-guid";

            var result = await _controller.FinalizaPedido(idPedido);

            var badRequestResult = result as BadRequestObjectResult;

            Assert.That(badRequestResult, Is.Not.Null);
        }

        [Test]

        public async Task FinalizaPedido_ReturnsOk_WhenSuccessful()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
                
            var result = await _controller.FinalizaPedido(idPedido);

            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task FinalizaPedido_ReturnsNotFound_When_PedidoNaoEncontradoException_IsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new PedidoNaoEncontradoException("Pedido nao encontrado"));

            var result = await _controller.FinalizaPedido(idPedido);

            var notFoundErrorResult = result as NotFoundObjectResult;
            Assert.That(notFoundErrorResult, Is.Not.Null);
            Assert.That(notFoundErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(notFoundErrorResult.Value, Is.EqualTo("Pedido nao encontrado"));
        }

        [Test]
        public async Task FinalizaPedido_ReturnsInternalServerError_When_AcompanhamentoNaoEncontradoException_IsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new AcompanhamentoNaoEncontradoException("Acompanhamento nao encontrado"));

            var result = await _controller.FinalizaPedido(idPedido);

            var internalServerErrorResult = result as ObjectResult;

            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(internalServerErrorResult.Value, Is.EqualTo("Acompanhamento nao encontrado"));
        }

        [Test]
        public async Task FinalizaPedido_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new OperacaoInvalidaException("Operacao invalida"));

            var result = await _controller.FinalizaPedido(idPedido);

            var badRequestErrorResult = result as BadRequestObjectResult;

            Assert.That(badRequestErrorResult, Is.Not.Null);
            Assert.That(badRequestErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task FinalizaPedido_ReturnsNotFound_When_AtualizarStatusException_IsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new AtualizarStatusException("Nao foi possivel atualizar o status"));

            var result = await _controller.FinalizaPedido(idPedido);

            var notFoundErrorResult = result as NotFoundObjectResult;

            Assert.That(notFoundErrorResult, Is.Not.Null);
            Assert.That(notFoundErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }

        [Test]
        public async Task FinalizaPedido_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            var idPedido = Guid.NewGuid().ToString();

            _acompanhamentoUseCasesMock
                .Setup(x => x.FinalizaPedidoAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            var result = await _controller.FinalizaPedido(idPedido);

            var internalServerErrorResult = result as ObjectResult;

            Assert.That(internalServerErrorResult, Is.Not.Null);
            Assert.That(internalServerErrorResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetAllPedidos_ReturnsOk_WhenSuccessful()
        {
            var pedidosMock = new List<AcompanhamentoDto>
            {
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Recebido },
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Pronto }

            };

            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosAsync())
                 .ReturnsAsync(pedidosMock);

            var result = await _controller.GetAllPedidos();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(pedidosMock));
        }

        [Test]
        public async Task GetAllPedidos_ReturnsInternalServerError_WhenExceptionsIsThrown()
        {
            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosAsync())
                 .ThrowsAsync(new Exception());

            var result = await _controller.GetAllPedidos();


            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetAllPedidosWithStatusEmPreparacao_ReturnsOk_WhenSuccessful()
        {
            var pedidosMock = new List<AcompanhamentoDto>
            {
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Preparacao },
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Preparacao }

            };

            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosByStatusAsync(Status.Preparacao))
                 .ReturnsAsync(pedidosMock);

            var result = await _controller.GetAllPedidosWithStatusEmPreparacao();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(pedidosMock));
        }

        [Test]
        public async Task GetAllPedidosWithStatusEmPreparacao_ReturnsInternalServerError_WhenExceptionsIsThrown()
        {
            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosByStatusAsync(Status.Preparacao))
                 .ThrowsAsync(new Exception());

            var result = await _controller.GetAllPedidosWithStatusEmPreparacao();


            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetAllPedidosWithStatusPronto_ReturnsOk_WhenSuccessful()
        {
            var pedidosMock = new List<AcompanhamentoDto>
            {
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Preparacao },
                new AcompanhamentoDto { IdPedido = Guid.NewGuid(), Status = Status.Preparacao }

            };

            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosByStatusAsync(Status.Pronto))
                 .ReturnsAsync(pedidosMock);

            var result = await _controller.GetAllPedidosWithStatusPronto();

            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.EqualTo(pedidosMock));
        }

        [Test]
        public async Task GetAllPedidosWithStatusPronto_ReturnsInternalServerError_WhenExceptionsIsThrown()
        {
            _acompanhamentoUseCasesMock
                 .Setup(x => x.GetAllPedidosByStatusAsync(Status.Pronto))
                 .ThrowsAsync(new Exception());

            var result = await _controller.GetAllPedidosWithStatusPronto();


            var okResult = result as ObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }
    }
}