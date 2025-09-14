using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromocaoController : ControllerBase
    {
        private readonly PromocaoService _promocaoService;

        public PromocaoController(PromocaoService promocaoService)
        {
            _promocaoService = promocaoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodas()
        {
            var resultado = await _promocaoService.ObterTodasAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _promocaoService.ObterPorIdAsync(id);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarPromocaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _promocaoService.CriarAsync(dto);
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPromocaoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _promocaoService.AtualizarAsync(id, dto);
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var resultado = await _promocaoService.ExcluirAsync(id);
            return Ok(resultado);
        }
    }
}
