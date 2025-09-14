using Dapper;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Interfaces.Repositories;
using GestaoPedidos.Infra.Data;

namespace GestaoPedidos.Infra.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public ProdutoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync() 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT Id, Sku, Nome, PrecoBase, Ativo, EstoqueAtual, CriadoEm, AtualizadoEm
                FROM Produtos 
                ORDER BY Nome";

            return await connection.QueryAsync<Produto>(sql);
        }

        public async Task<Produto?> ObterPorIdAsync(int id) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT Id, Sku, Nome, PrecoBase, Ativo, EstoqueAtual, CriadoEm, AtualizadoEm
                FROM Produtos 
                WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Id = id });
        }

        public async Task<Produto?> ObterPorSkuAsync(string sku)
        {
            using var connection = _connectionFactory.CreateConnection(); 
            const string sql = @"
                SELECT Id, Sku, Nome, PrecoBase, Ativo, EstoqueAtual, CriadoEm, AtualizadoEm
                FROM Produtos 
                WHERE Sku = @Sku";

            return await connection.QueryFirstOrDefaultAsync<Produto>(sql, new { Sku = sku });
        }

        public async Task<int> CriarAsync(Produto produto) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                INSERT INTO Produtos (Sku, Nome, PrecoBase, Ativo, EstoqueAtual, CriadoEm)
                VALUES (@Sku, @Nome, @PrecoBase, @Ativo, @EstoqueAtual, @CriadoEm);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.QuerySingleAsync<int>(sql, produto);
        }

        public async Task<bool> AtualizarAsync(Produto produto) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE Produtos 
                SET Nome = @Nome, 
                    PrecoBase = @PrecoBase, 
                    Ativo = @Ativo, 
                    EstoqueAtual = @EstoqueAtual,
                    AtualizadoEm = @AtualizadoEm
                WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, produto);
            return rowsAffected > 0;
        }

        public async Task<bool> ExcluirAsync(int id) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM Produtos WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> BaixarEstoque(int produtoId, int quantidade) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE Produtos 
                SET EstoqueAtual = EstoqueAtual - @Quantidade,
                    AtualizadoEm = GETDATE()
                WHERE Id = @ProdutoId AND EstoqueAtual >= @Quantidade";

            var rowsAffected = await connection.ExecuteAsync(sql, new { ProdutoId = produtoId, Quantidade = quantidade });
            return rowsAffected > 0;
        }

        public async Task<bool> EstornarEstoque(int produtoId, int quantidade) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE Produtos 
                SET EstoqueAtual = EstoqueAtual + @Quantidade,
                    AtualizadoEm = GETDATE()
                WHERE Id = @ProdutoId";

            var rowsAffected = await connection.ExecuteAsync(sql, new { ProdutoId = produtoId, Quantidade = quantidade });
            return rowsAffected > 0;
        }
    }
}
