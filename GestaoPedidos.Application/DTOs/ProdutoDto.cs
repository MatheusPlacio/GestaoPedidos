namespace GestaoPedidos.Application.DTOs
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public decimal PrecoFinal { get; set; }
        public bool Ativo { get; set; }
        public int EstoqueAtual { get; set; }
        public DateTime CriadoEm { get; set; }
    }

    public class CriarProdutoDto
    {
        public string Sku { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; } = true;
        public int EstoqueAtual { get; set; }
    }

    public class AtualizarProdutoDto
    {
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public bool Ativo { get; set; }
        public int EstoqueAtual { get; set; }
    }
}
