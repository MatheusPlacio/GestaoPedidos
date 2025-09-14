using GestaoPedidos.Domain.Entities;

namespace GestaoPedidos.Domain.Interfaces.Repositories
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task<Produto?> ObterPorIdAsync(int id);
        Task<Produto?> ObterPorSkuAsync(string sku);
        Task<int> CriarAsync(Produto produto);
        Task<bool> AtualizarAsync(Produto produto);
        Task<bool> ExcluirAsync(int id);
        Task<bool> BaixarEstoque(int produtoId, int quantidade);
        Task<bool> EstornarEstoque(int produtoId, int quantidade);
    }
}
