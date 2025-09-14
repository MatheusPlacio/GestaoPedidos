using Dapper;
using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Enums;
using GestaoPedidos.Domain.Interfaces.Repositories;
using GestaoPedidos.Infra.Data;

namespace GestaoPedidos.Infra.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public PedidoRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Pedido>> ObterTodosAsync() 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT Id, Numero, ClienteId, Status, Itens, TotalBruto, Desconto, TotalLiquido, 
                       CriadoEm, ConfirmadoEm, CanceladoEm, FaturadoEm
                FROM Pedidos 
                ORDER BY CriadoEm DESC";

            return await connection.QueryAsync<Pedido>(sql);
        }

        public async Task<Pedido?> ObterPorIdAsync(int id) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                SELECT Id, Numero, ClienteId, Status, Itens, TotalBruto, Desconto, TotalLiquido, 
                       CriadoEm, ConfirmadoEm, CanceladoEm, FaturadoEm
                FROM Pedidos 
                WHERE Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<Pedido>(sql, new { Id = id });
        }

        public async Task<int> CriarAsync(Pedido pedido) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                INSERT INTO Pedidos (Numero, ClienteId, Status, Itens, TotalBruto, Desconto, TotalLiquido, CriadoEm)
                VALUES (@Numero, @ClienteId, @Status, @Itens, @TotalBruto, @Desconto, @TotalLiquido, @CriadoEm);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.QuerySingleAsync<int>(sql, pedido);
        }

        public async Task<string> GerarProximoNumero() 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                DECLARE @ProximoNumero INT
                SELECT @ProximoNumero = ISNULL(MAX(CAST(SUBSTRING(Numero, 4, LEN(Numero)) AS INT)), 0) + 1
                FROM Pedidos 
                WHERE Numero LIKE 'PED%'
                
                SELECT 'PED' + RIGHT('000000' + CAST(@ProximoNumero AS VARCHAR), 6)";

            return await connection.QuerySingleAsync<string>(sql);
        }

        public async Task<bool> ConfirmarPedido(int id) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE Pedidos 
                SET Status = 2, -- Confirmado
                    ConfirmadoEm = GETDATE()
                WHERE Id = @Id AND Status = 1"; // Apenas se estiver em Rascunho

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> CancelarPedido(int id) 
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE Pedidos 
                SET Status = 3, -- Cancelado
                    CanceladoEm = GETDATE()
                WHERE Id = @Id AND Status != 4"; // Não pode cancelar se já faturado

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}
