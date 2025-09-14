using System.Data;
using Dapper;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Interfaces.Repositories;
using GestaoPedidos.Infra.Data;

namespace GestaoPedidos.Infra.Repositories
{
    public class PromocaoRepository : IPromocaoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PromocaoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Promocao>> ObterTodasAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Promocao>(
                "sp_ObterTodasPromocoes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Promocao?> ObterPorIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Promocao>(
                "sp_ObterPromocaoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CriarAsync(Promocao promocao)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<int>(
                "sp_CriarPromocao",
                new
                {
                    promocao.Nome,
                    promocao.Descricao,
                    promocao.ProdutoId,
                    promocao.Tipo,
                    promocao.Valor,
                    promocao.DataInicio,
                    promocao.DataFim,
                    promocao.Ativo,
                    promocao.CriadoEm
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> AtualizarAsync(Promocao promocao)
        {
            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteScalarAsync<int>(
                "sp_AtualizarPromocao",
                new
                {
                    promocao.Id,
                    promocao.Nome,
                    promocao.Descricao,
                    promocao.ProdutoId,
                    promocao.Tipo,
                    promocao.Valor,
                    promocao.DataInicio,
                    promocao.DataFim,
                    promocao.Ativo
                },
                commandType: CommandType.StoredProcedure);

            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var linhasAfetadas = await connection.ExecuteScalarAsync<int>(
                "sp_ExcluirPromocao",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return linhasAfetadas > 0;
        }

        public async Task<List<Promocao>> ObterPromocoesPorProduto(int produtoId)
        {
            using var connection = _connectionFactory.CreateConnection();
            var promocoes = await connection.QueryAsync<Promocao>(
                "sp_ObterPromocoesPorProduto",
                new { ProdutoId = produtoId },
                commandType: CommandType.StoredProcedure);

            return promocoes.ToList();
        }
    }
}
