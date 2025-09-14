using System.Data;

namespace GestaoPedidos.Infra.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
