using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.DTOs.Common;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Interfaces.Repositories;

namespace GestaoPedidos.Application.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IPromocaoRepository _promocaoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, IPromocaoRepository promocaoRepository)
        {
            _produtoRepository = produtoRepository;
            _promocaoRepository = promocaoRepository;
        }

        public async Task<ResultDto<List<ProdutoDto>>> ObterTodosAsync()
        {
            try
            {
                var produtos = await _produtoRepository.ObterTodosAsync();
                var produtosDto = new List<ProdutoDto>();

                foreach (var produto in produtos)
                {
                    var promocoes = await _promocaoRepository.ObterPromocoesPorProduto(produto.Id);
                    var precoFinal = produto.CalcularPrecoFinal(promocoes);

                    produtosDto.Add(new ProdutoDto
                    {
                        Id = produto.Id,
                        Sku = produto.Sku,
                        Nome = produto.Nome,
                        PrecoBase = produto.PrecoBase,
                        PrecoFinal = precoFinal,
                        Ativo = produto.Ativo,
                        EstoqueAtual = produto.EstoqueAtual,
                        CriadoEm = produto.CriadoEm
                    });
                }

                return ResultDto<List<ProdutoDto>>.SucessoComDados(produtosDto);
            }
            catch (Exception ex)
            {
                return ResultDto<List<ProdutoDto>>.Erro($"Erro ao obter produtos: {ex.Message}");
            }
        }

        public async Task<ResultDto<ProdutoDto>> ObterPorIdAsync(int id)
        {
            try
            {
                var produto = await _produtoRepository.ObterPorIdAsync(id);
                if (produto == null)
                    return ResultDto<ProdutoDto>.Erro("Produto não encontrado");

                var promocoes = await _promocaoRepository.ObterPromocoesPorProduto(produto.Id);
                var precoFinal = produto.CalcularPrecoFinal(promocoes);

                var produtoDto = new ProdutoDto
                {
                    Id = produto.Id,
                    Sku = produto.Sku,
                    Nome = produto.Nome,
                    PrecoBase = produto.PrecoBase,
                    PrecoFinal = precoFinal,
                    Ativo = produto.Ativo,
                    EstoqueAtual = produto.EstoqueAtual,
                    CriadoEm = produto.CriadoEm
                };

                return ResultDto<ProdutoDto>.SucessoComDados(produtoDto);
            }
            catch (Exception ex)
            {
                return ResultDto<ProdutoDto>.Erro($"Erro ao obter produto: {ex.Message}");
            }
        }

        public async Task<ResultDto<int>> CriarAsync(CriarProdutoDto dto)
        {
            try
            {
                // Verificar se SKU já existe
                var produtoExistente = await _produtoRepository.ObterPorSkuAsync(dto.Sku);
                if (produtoExistente != null)
                    return ResultDto<int>.Erro("SKU já existe");

                var produto = new Produto
                {
                    Sku = dto.Sku,
                    Nome = dto.Nome,
                    PrecoBase = dto.PrecoBase,
                    Ativo = dto.Ativo,
                    EstoqueAtual = dto.EstoqueAtual,
                    CriadoEm = DateTime.Now
                };

                var id = await _produtoRepository.CriarAsync(produto);
                return ResultDto<int>.SucessoComDados(id, "Produto criado com sucesso");
            }
            catch (Exception ex)
            {
                return ResultDto<int>.Erro($"Erro ao criar produto: {ex.Message}");
            }
        }

        public async Task<ResultDto> AtualizarAsync(int id, AtualizarProdutoDto dto)
        {
            try
            {
                var produto = await _produtoRepository.ObterPorIdAsync(id);
                if (produto == null)
                    return ResultDto.Erro("Produto não encontrado");

                produto.Nome = dto.Nome;
                produto.PrecoBase = dto.PrecoBase;
                produto.Ativo = dto.Ativo;
                produto.EstoqueAtual = dto.EstoqueAtual;
                produto.AtualizadoEm = DateTime.Now;

                var sucesso = await _produtoRepository.AtualizarAsync(produto);
                return sucesso ? ResultDto.Ok("Produto atualizado com sucesso") : ResultDto.Erro("Erro ao atualizar produto");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao atualizar produto: {ex.Message}");
            }
        }

        public async Task<ResultDto> ExcluirAsync(int id)
        {
            try
            {
                var produto = await _produtoRepository.ObterPorIdAsync(id);
                if (produto == null)
                    return ResultDto.Erro("Produto não encontrado");

                var sucesso = await _produtoRepository.ExcluirAsync(id);
                return sucesso ? ResultDto.Ok("Produto excluído com sucesso") : ResultDto.Erro("Erro ao excluir produto");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao excluir produto: {ex.Message}");
            }
        }
    }
}
