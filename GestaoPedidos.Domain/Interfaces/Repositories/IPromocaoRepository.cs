using GestaoPedidos.Domain.Entities;

namespace GestaoPedidos.Domain.Interfaces.Repositories
{
    public interface IPromocaoRepository
    {
        Task<IEnumerable<Promocao>> ObterTodasAsync();
        Task<Promocao?> ObterPorIdAsync(int id);
        Task<int> CriarAsync(Promocao promocao);
        Task<bool> AtualizarAsync(Promocao promocao);
        Task<bool> ExcluirAsync(int id);
        Task<List<Promocao>> ObterPromocoesPorProduto(int produtoId);
    }
}
