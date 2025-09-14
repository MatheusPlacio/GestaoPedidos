using GestaoPedidos.Domain.Entities;
using GestaoPedidos.Domain.Enums;

namespace GestaoPedidos.Domain.Interfaces.Repositories
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ObterTodosAsync();
        Task<Pedido?> ObterPorIdAsync(int id);
        Task<int> CriarAsync(Pedido pedido);
        Task<string> GerarProximoNumero();
        Task<bool> ConfirmarPedido(int id);
        Task<bool> CancelarPedido(int id);
    }
}
