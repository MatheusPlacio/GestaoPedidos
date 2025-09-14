using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _produtoService;

        public ProdutoController(ProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var resultado = await _produtoService.ObterTodosAsync();
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var resultado = await _produtoService.ObterPorIdAsync(id);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CriarProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _produtoService.CriarAsync(dto);
            return Ok(resultado);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _produtoService.AtualizarAsync(id, dto);
            return Ok(resultado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            var resultado = await _produtoService.ExcluirAsync(id);
            return Ok(resultado);
        }
    }
}
