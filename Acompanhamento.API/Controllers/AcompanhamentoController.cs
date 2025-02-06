using Acompanhamento.Core.Entities.Enums;
using Acompanhamento.UseCases.Exceptions;
using Acompanhamento.UseCases.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Acompanhamento.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AcompanhamentoController : ControllerBase
    {
       private readonly IAcompanhamentoUseCases AcompanhamentoUseCases;
       public AcompanhamentoController(IAcompanhamentoUseCases acompanhamentoUseCases) 
       { 
            AcompanhamentoUseCases = acompanhamentoUseCases;
       }

        [HttpPost("{idPedido}/declararPronto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AtualizaStatusComoPronto([FromRoute] string idPedido)
        {
            if (!Guid.TryParse(idPedido, out var idPedidoGuid)) return BadRequest("O id do pagamento nao pode ser nulo.");

            try
            {
                await AcompanhamentoUseCases.AtualizaStatusComoProntoAsync(idPedidoGuid);

                return Ok();
            }
            catch (PedidoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AcompanhamentoNaoEncontradoException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (OperacaoInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AtualizarStatusException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }

        }

        [HttpPost("{idPedido}/finalizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FinalizaPedido([FromRoute] string idPedido)
        {
            if (!Guid.TryParse(idPedido, out var idPedidoGuid)) return BadRequest("O id do pagamento nao pode ser nulo.");

            try
            {
                await AcompanhamentoUseCases.FinalizaPedidoAsync(idPedidoGuid);

                return Ok();
            }
            catch (PedidoNaoEncontradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AcompanhamentoNaoEncontradoException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            catch (OperacaoInvalidaException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AtualizarStatusException ex)
            {
                return NotFound(ex.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet("todos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPedidos()
        {
            try
            {
                var pedidos = await AcompanhamentoUseCases.GetAllPedidosAsync();

                return Ok(pedidos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet("recebidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPedidosWithStatusRecebido()
        {
            try
            {
                var pedidos = await AcompanhamentoUseCases.GetAllPedidosByStatusAsync(Status.Recebido);

                return Ok(pedidos);
            }
            catch (Exception ex)
            {          
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet("emPreparacao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPedidosWithStatusEmPreparacao()
        {
            try
            {
                var pedidos = await AcompanhamentoUseCases.GetAllPedidosByStatusAsync(Status.Preparacao);

                return Ok(pedidos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }

        [HttpGet("prontos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPedidosWithStatusPronto()
        {
            try
            {
                var pedidos = await AcompanhamentoUseCases.GetAllPedidosByStatusAsync(Status.Pronto);

                return Ok(pedidos);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao processar a requisição, tente novamente mais tarde.");
            }
        }
    }
}
