using GestaoPedidos.Application.DTOs;
using GestaoPedidos.Application.DTOs.Common;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Interfaces.Repositories;

namespace GestaoPedidos.Application.Services
{
    public class PromocaoService
    {
        private readonly IPromocaoRepository _promocaoRepository;
        private readonly IProdutoRepository _produtoRepository;

        public PromocaoService(IPromocaoRepository promocaoRepository, IProdutoRepository produtoRepository)
        {
            _promocaoRepository = promocaoRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<ResultDto<List<PromocaoDto>>> ObterTodasAsync()
        {
            try
            {
                var promocoes = await _promocaoRepository.ObterTodasAsync();
                var promocoesDto = promocoes.Select(p => new PromocaoDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    ProdutoId = p.ProdutoId,
                    Tipo = p.Tipo,
                    Valor = p.Valor,
                    DataInicio = p.DataInicio,
                    DataFim = p.DataFim,
                    Ativo = p.Ativo,
                    CriadoEm = p.CriadoEm
                }).ToList();

                return ResultDto<List<PromocaoDto>>.SucessoComDados(promocoesDto);
            }
            catch (Exception ex)
            {
                return ResultDto<List<PromocaoDto>>.Erro($"Erro ao obter promoções: {ex.Message}");
            }
        }

        public async Task<ResultDto<PromocaoDto>> ObterPorIdAsync(int id)
        {
            try
            {
                var promocao = await _promocaoRepository.ObterPorIdAsync(id);
                if (promocao == null)
                    return ResultDto<PromocaoDto>.Erro("Promoção não encontrada");

                var promocaoDto = new PromocaoDto
                {
                    Id = promocao.Id,
                    Nome = promocao.Nome,
                    Descricao = promocao.Descricao,
                    Tipo = promocao.Tipo,
                    Valor = promocao.Valor,
                    DataInicio = promocao.DataInicio,
                    DataFim = promocao.DataFim,
                    Ativo = promocao.Ativo,
                    CriadoEm = promocao.CriadoEm
                };

                return ResultDto<PromocaoDto>.SucessoComDados(promocaoDto);
            }
            catch (Exception ex)
            {
                return ResultDto<PromocaoDto>.Erro($"Erro ao obter promoção: {ex.Message}");
            }
        }

     public async Task<ResultDto<int>> CriarAsync(CriarPromocaoDto dto)
     {
         try
         {
             // Buscar o produto para obter o nome
             var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
             if (produto == null)
                 return ResultDto<int>.Erro("Produto não encontrado");
                 
             // Verificar se o produto já possui uma promoção ativa
             var promocoesExistentes = await _promocaoRepository.ObterPromocoesPorProduto(dto.ProdutoId);
             if (promocoesExistentes.Any(p => p.Ativo && 
                 ((p.DataInicio <= dto.DataFim && p.DataFim >= dto.DataInicio) || 
                  (dto.DataInicio <= p.DataFim && dto.DataFim >= p.DataInicio))))
             {
                 return ResultDto<int>.Erro($"O produto '{produto.Nome}' já possui uma promoção ativa no período informado");
             }

             var promocao = new Promocao
             {
                 Nome = produto.Nome, 
                 Descricao = dto.Descricao,
                 ProdutoId = dto.ProdutoId,
                 Tipo = dto.Tipo,
                 Valor = dto.Valor,
                 DataInicio = dto.DataInicio,
                 DataFim = dto.DataFim,
                 Ativo = dto.Ativo,
                 CriadoEm = DateTime.Now
             };

             var id = await _promocaoRepository.CriarAsync(promocao);
             return ResultDto<int>.SucessoComDados(id, "Promoção criada com sucesso");
         }
         catch (Exception ex)
         {
             return ResultDto<int>.Erro($"Erro ao criar promoção: {ex.Message}");
         }
     }

        public async Task<ResultDto> AtualizarAsync(int id, AtualizarPromocaoDto dto)
        {
            try
            {
                var promocao = await _promocaoRepository.ObterPorIdAsync(id);
                if (promocao == null)
                    return ResultDto.Erro("Promoção não encontrada");

                var produto = await _produtoRepository.ObterPorIdAsync(dto.ProdutoId);
                if (produto == null)
                    return ResultDto.Erro("Produto não encontrado");
                    
                // Verificar se o produto já possui outra promoção ativa no mesmo período
                var promocoesExistentes = await _promocaoRepository.ObterPromocoesPorProduto(dto.ProdutoId);
                if (promocoesExistentes.Any(p => p.Id != id && p.Ativo && 
                    ((p.DataInicio <= dto.DataFim && p.DataFim >= dto.DataInicio) || 
                     (dto.DataInicio <= p.DataFim && dto.DataFim >= p.DataInicio))))
                {
                    return ResultDto.Erro($"O produto '{produto.Nome}' já possui outra promoção ativa no período informado");
                }

                promocao.Nome = produto.Nome; 
                promocao.Descricao = dto.Descricao;
                promocao.ProdutoId = dto.ProdutoId;
                promocao.Tipo = dto.Tipo;
                promocao.Valor = dto.Valor;
                promocao.DataInicio = dto.DataInicio;
                promocao.DataFim = dto.DataFim;
                promocao.Ativo = dto.Ativo;

                var sucesso = await _promocaoRepository.AtualizarAsync(promocao);
                return sucesso ? ResultDto.Ok("Promoção atualizada com sucesso") : ResultDto.Erro("Erro ao atualizar promoção");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao atualizar promoção: {ex.Message}");
            }
        }

        public async Task<ResultDto> ExcluirAsync(int id)
        {
            try
            {
                var promocao = await _promocaoRepository.ObterPorIdAsync(id);
                if (promocao == null)
                    return ResultDto.Erro("Promoção não encontrada");

                var sucesso = await _promocaoRepository.ExcluirAsync(id);
                return sucesso ? ResultDto.Ok("Promoção excluída com sucesso") : ResultDto.Erro("Erro ao excluir promoção");
            }
            catch (Exception ex)
            {
                return ResultDto.Erro($"Erro ao excluir promoção: {ex.Message}");
            }
        }
    }
}
