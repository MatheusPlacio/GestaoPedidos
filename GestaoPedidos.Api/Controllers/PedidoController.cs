using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _pedidoService;

        public PedidoController(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var resultado = await _pedidoService.ObterTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _pedidoService.ObterPorIdAsync(id);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _pedidoService.CriarAsync(dto);
            return Ok(resultado);
        }

        [HttpPut("{id}/confirmar")]
        public async Task<IActionResult> Confirmar(int id)
        {
            var resultado = await _pedidoService.ConfirmarAsync(id);
            return Ok(resultado);
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _pedidoService.CancelarAsync(id);
            return Ok(resultado);
        }
    }
}
